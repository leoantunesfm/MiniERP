using FluentAssertions;
using MiniERP.Application.DTOs;
using MiniERP.Application.Interfaces;
using MiniERP.Application.UseCases;
using MiniERP.Domain.Exceptions;
using NSubstitute;
using Xunit;

namespace MiniERP.Application.Tests.UseCases;

public class GetCompanyDataByCnpjUseCaseTests
{
    private readonly IReceitaWsClient _receitaWsClientMock;
    private readonly GetCompanyDataByCnpjUseCase _sut;

    public GetCompanyDataByCnpjUseCaseTests()
    {
        _receitaWsClientMock = Substitute.For<IReceitaWsClient>();
        _sut = new GetCompanyDataByCnpjUseCase(_receitaWsClientMock);
    }

    [Fact]
    public async Task ExecuteAsync_DeveLancarExcecao_QuandoCnpjNaoInformado()
    {
        var act = async () => await _sut.ExecuteAsync("");

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("CNPJ não informado.");
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarObjetoVazio_QuandoApiRetornarNull()
    {
        _receitaWsClientMock.ConsultarCnpjAsync("123").Returns((ConsultaCnpjResponseDto?)null);

        var result = await _sut.ExecuteAsync("123");

        result.Should().NotBeNull();
        result.Nome.Should().BeNull();
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarDados_QuandoApiRetornarSucesso()
    {
        var dto = new ConsultaCnpjResponseDto { Nome = "Empresa Teste", Uf = "SP" };
        _receitaWsClientMock.ConsultarCnpjAsync("123").Returns(dto);

        var result = await _sut.ExecuteAsync("123");

        result.Should().NotBeNull();
        result.Nome.Should().Be("Empresa Teste");
        result.Uf.Should().Be("SP");
    }
}