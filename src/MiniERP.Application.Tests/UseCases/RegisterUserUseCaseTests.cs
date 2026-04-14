using FluentAssertions;
using MiniERP.Application.DTOs;
using MiniERP.Application.UseCases;
using MiniERP.Domain.Entities;
using MiniERP.Domain.Exceptions;
using MiniERP.Domain.Interfaces;
using MiniERP.Domain.ValueObjects;
using MiniERP.Domain.Common;
using NSubstitute;
using Xunit;

namespace MiniERP.Application.Tests.UseCases;

public class RegisterUserUseCaseTests
{
    private readonly IUsuarioRepository _usuarioRepositoryMock;
    private readonly IPerfilRepository _perfilRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IPasswordHasher _passwordHasherMock;
    private readonly IMessagePublisher _messagePublisherMock;
    private readonly RegisterUserUseCase _sut;

    public RegisterUserUseCaseTests()
    {
        _usuarioRepositoryMock = Substitute.For<IUsuarioRepository>();
        _perfilRepositoryMock = Substitute.For<IPerfilRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _passwordHasherMock = Substitute.For<IPasswordHasher>();
        _messagePublisherMock = Substitute.For<IMessagePublisher>();

        _sut = new RegisterUserUseCase(
            _usuarioRepositoryMock,
            _perfilRepositoryMock,
            _unitOfWorkMock,
            _passwordHasherMock,
            _messagePublisherMock);
    }

    [Fact]
    public async Task ExecuteAsync_DeveLancarExcecao_QuandoEmailJaExistir()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var request = new RegisterUserRequestDto { Email = "teste@teste.com" };
        
        var usuarioExistente = new Usuario(tenantId, "Teste", new Email(request.Email), "hash");
        _usuarioRepositoryMock.GetByEmailAsync(request.Email).Returns(usuarioExistente);

        // Act
        var act = async () => await _sut.ExecuteAsync(tenantId, request);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("Este e-mail já está em uso no sistema.");
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarSucessoEPublicarMensagem_QuandoFluxoForValido()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var perfilId = Guid.NewGuid();
        var request = new RegisterUserRequestDto 
        { 
            Nome = "Novo Usuario", 
            Email = "novo@teste.com", 
            PerfilId = perfilId 
        };

        _usuarioRepositoryMock.GetByEmailAsync(request.Email).Returns((Usuario?)null);
            
        var perfilMock = new Perfil("Operador", "Operador do sistema.");
        typeof(BaseEntity).GetProperty("Id")?.SetValue(perfilMock, perfilId);
        
        _perfilRepositoryMock.GetByIdAsync(perfilId).Returns(perfilMock);
        _passwordHasherMock.Hash(Arg.Any<string>()).Returns("senhaHashada");

        // Act
        var response = await _sut.ExecuteAsync(tenantId, request);

        // Assert
        response.Should().NotBeNull();
        response.Nome.Should().Be(request.Nome);
        response.Email.Should().Be(request.Email);

        await _usuarioRepositoryMock.Received(1).AddAsync(Arg.Any<Usuario>());
        await _unitOfWorkMock.Received(1).CommitAsync();
        await _messagePublisherMock.Received(1).PublicarAsync(Arg.Any<EnviarEmailMessage>(), "email_queue");
    }
}