using FluentAssertions;
using MiniERP.Application.UseCases;
using MiniERP.Domain.Entities;
using MiniERP.Domain.Exceptions;
using MiniERP.Domain.Interfaces;
using MiniERP.Domain.ValueObjects;
using NSubstitute;
using Xunit;

namespace MiniERP.Application.Tests.UseCases;

public class ConfirmEmailUseCaseTests
{
    private readonly IEmpresaRepository _empresaRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly ConfirmEmailUseCase _sut;

    public ConfirmEmailUseCaseTests()
    {
        _empresaRepositoryMock = Substitute.For<IEmpresaRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new ConfirmEmailUseCase(_empresaRepositoryMock, _unitOfWorkMock);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task ExecuteAsync_DeveLancarExcecao_QuandoTokenForInvalido(string tokenInvalido)
    {
        var act = async () => await _sut.ExecuteAsync(tokenInvalido);

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("Token não informado.");
    }

    [Fact]
    public async Task ExecuteAsync_DeveLancarExcecao_QuandoEmpresaNaoEncontrada()
    {
        _empresaRepositoryMock.GetByTokenAsync(Arg.Any<string>()).Returns((Empresa?)null);

        var act = async () => await _sut.ExecuteAsync("token_falso");

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("Token de confirmação inválido ou expirado.");
    }

    [Fact]
    public async Task ExecuteAsync_DeveConfirmarEmail_QuandoTokenForValido()
    {
        var empresa = new Empresa(new Cnpj("18045008000150")); // Status inicial: AguardandoConfirmacaoEmail
        _empresaRepositoryMock.GetByTokenAsync("token_valido").Returns(empresa);
        _unitOfWorkMock.CommitAsync().Returns(true);

        var result = await _sut.ExecuteAsync("token_valido");

        result.Should().BeTrue();
        empresa.TokenConfirmacaoEmail.Should().BeNull();
        await _unitOfWorkMock.Received(1).CommitAsync();
    }
}