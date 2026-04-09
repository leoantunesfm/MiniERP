using System.Text.Json;
using MiniERP.Application.DTOs;
using MiniERP.Domain.Entities;
using MiniERP.Domain.Exceptions;
using MiniERP.Domain.Interfaces;
using MiniERP.Domain.ValueObjects;

namespace MiniERP.Application.UseCases;

public class RegisterUserUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPerfilRepository _perfilRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMessagePublisher _messagePublisher;

    public RegisterUserUseCase(
        IUsuarioRepository usuarioRepository,
        IPerfilRepository perfilRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IMessagePublisher messagePublisher)
    {
        _usuarioRepository = usuarioRepository;
        _perfilRepository = perfilRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _messagePublisher = messagePublisher;
    }

    public async Task<RegisterUserResponseDto> ExecuteAsync(Guid tenantId, RegisterUserRequestDto request)
    {
        var existingUser = await _usuarioRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new DomainException("Este e-mail já está em uso no sistema.");

        var perfil = await _perfilRepository.GetByIdAsync(request.PerfilId);
        if (perfil == null)
            throw new DomainException("O perfil selecionado não foi encontrado.");

        var senhaTemporaria = Guid.NewGuid().ToString("N")[..8];
        var senhaHash = _passwordHasher.Hash(senhaTemporaria);

        var emailVo = new Email(request.Email);
        var novoUsuario = new Usuario(tenantId, request.Nome, emailVo, senhaHash);

        var usuarioPerfil = new UsuarioPerfil(novoUsuario.Id, perfil.Id);
        novoUsuario.UsuarioPerfis.Add(usuarioPerfil);

        await _usuarioRepository.AddAsync(novoUsuario);
        await _unitOfWork.CommitAsync();

        var corpoEmail = $@"
            <h3>Olá {request.Nome},</h3>
            <p>Você foi convidado para acessar o MiniERP.</p>
            <p>Sua senha de acesso temporária é: <strong>{senhaTemporaria}</strong></p>
            <p>Recomendamos que você altere sua senha no primeiro acesso.</p>";

        var mensagem = new EnviarEmailMessage(request.Email, "Bem-vindo ao MiniERP", corpoEmail);
        await _messagePublisher.PublicarAsync(mensagem, "email_queue");

        return new RegisterUserResponseDto
        {
            Id = novoUsuario.Id,
            Nome = novoUsuario.Nome,
            Email = request.Email
        };
    }
}