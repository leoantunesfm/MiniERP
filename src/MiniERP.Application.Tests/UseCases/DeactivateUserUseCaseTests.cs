using FluentAssertions;
using MiniERP.Application.UseCases;
using MiniERP.Domain.Entities;
using MiniERP.Domain.Exceptions;
using MiniERP.Domain.Interfaces;
using MiniERP.Domain.ValueObjects;
using MiniERP.Domain.Common;
using NSubstitute;
using Xunit;

namespace MiniERP.Application.Tests.UseCases;

public class DeactivateUserUseCaseTests
{
    private readonly IUsuarioRepository _usuarioRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly DeactivateUserUseCase _sut;

    public DeactivateUserUseCaseTests()
    {
        _usuarioRepositoryMock = Substitute.For<IUsuarioRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        
        _sut = new DeactivateUserUseCase(_usuarioRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task ExecuteAsync_DeveLancarExcecao_QuandoUsuarioForAdmin()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        
        var usuario = new Usuario(tenantId, "Admin User", new Email("admin@teste.com"), "hash");
        var perfilAdmin = new Perfil("Admin","Administrador do sistema.");
        
        typeof(BaseEntity).GetProperty("Id")?.SetValue(perfilAdmin, Guid.NewGuid());
        var usuarioPerfil = new UsuarioPerfil(usuario.Id, perfilAdmin.Id);
        typeof(UsuarioPerfil).GetProperty("Perfil")?.SetValue(usuarioPerfil, perfilAdmin);
        usuario.UsuarioPerfis.Add(usuarioPerfil);

        _usuarioRepositoryMock.GetByIdAndTenantWithPerfisAsync(usuarioId, tenantId).Returns(usuario);

        // Act
        var act = async () => await _sut.ExecuteAsync(tenantId, usuarioId);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("Não é possível inativar um usuário com perfil de Administrador.");
    }

    [Fact]
    public async Task ExecuteAsync_DeveInativarEComitar_QuandoUsuarioNaoForAdmin()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        
        var usuario = new Usuario(tenantId, "Operador User", new Email("op@teste.com"), "hash");
        var perfilOperador = new Perfil("Operador","Operador do sistema.");
        
        typeof(BaseEntity).GetProperty("Id")?.SetValue(perfilOperador, Guid.NewGuid());
        var usuarioPerfil = new UsuarioPerfil(usuario.Id, perfilOperador.Id);
        typeof(UsuarioPerfil).GetProperty("Perfil")?.SetValue(usuarioPerfil, perfilOperador);
        usuario.UsuarioPerfis.Add(usuarioPerfil);

        _usuarioRepositoryMock.GetByIdAndTenantWithPerfisAsync(usuarioId, tenantId).Returns(usuario);

        // Act
        await _sut.ExecuteAsync(tenantId, usuarioId);

        // Assert
        usuario.Ativo.Should().BeFalse();
        await _unitOfWorkMock.Received(1).CommitAsync();
    }
}