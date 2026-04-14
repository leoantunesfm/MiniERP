using FluentAssertions;
using MiniERP.Application.UseCases;
using MiniERP.Domain.Common;
using MiniERP.Domain.Entities;
using MiniERP.Domain.Interfaces;
using NSubstitute;
using Xunit;

namespace MiniERP.Application.Tests.UseCases;

public class ListProfilesUseCaseTests
{
    private readonly IPerfilRepository _perfilRepositoryMock;
    private readonly ListProfilesUseCase _sut;

    public ListProfilesUseCaseTests()
    {
        _perfilRepositoryMock = Substitute.For<IPerfilRepository>();
        _sut = new ListProfilesUseCase(_perfilRepositoryMock);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarListaDePerfis_QuandoExistiremNaBase()
    {
        // Arrange
        var perfilAdmin = new Perfil("Admin", "Administrador do Sistema");
        var perfilGerente = new Perfil("Gerente", "Gestão do negócio");

        typeof(BaseEntity).GetProperty("Id")?.SetValue(perfilAdmin, Guid.NewGuid());
        typeof(BaseEntity).GetProperty("Id")?.SetValue(perfilGerente, Guid.NewGuid());

        var perfisMock = new List<Perfil> { perfilAdmin, perfilGerente };

        _perfilRepositoryMock.GetAllAsync().Returns(perfisMock);

        // Act
        var result = await _sut.ExecuteAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var resultList = result.ToList();
        
        resultList[0].Id.Should().Be(perfilAdmin.Id);
        resultList[0].Nome.Should().Be("Admin");
        resultList[0].Descricao.Should().Be("Administrador do Sistema");

        resultList[1].Id.Should().Be(perfilGerente.Id);
        resultList[1].Nome.Should().Be("Gerente");
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarListaVazia_QuandoNaoExistiremPerfis()
    {
        // Arrange
        _perfilRepositoryMock.GetAllAsync().Returns(new List<Perfil>());

        // Act
        var result = await _sut.ExecuteAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}