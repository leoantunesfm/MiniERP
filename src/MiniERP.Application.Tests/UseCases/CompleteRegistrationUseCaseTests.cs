using FluentAssertions;
using MiniERP.Application.DTOs;
using MiniERP.Application.UseCases;
using MiniERP.Domain.Entities;
using MiniERP.Domain.Enums;
using MiniERP.Domain.Exceptions;
using MiniERP.Domain.Interfaces;
using MiniERP.Domain.ValueObjects;
using NSubstitute;
using Xunit;

namespace MiniERP.Application.Tests.UseCases;

public class CompleteRegistrationUseCaseTests
{
    private readonly IEmpresaRepository _empresaRepositoryMock;
    private readonly IStorageService _storageServiceMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CompleteRegistrationUseCase _sut;

    public CompleteRegistrationUseCaseTests()
    {
        _empresaRepositoryMock = Substitute.For<IEmpresaRepository>();
        _storageServiceMock = Substitute.For<IStorageService>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new CompleteRegistrationUseCase(_empresaRepositoryMock, _storageServiceMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task ExecuteAsync_DeveLancarExcecao_QuandoListaDeDocumentosEstiverVazia()
    {
        var request = new CompleteRegistrationRequestDto { EmpresaId = Guid.NewGuid() };
        var documentos = new List<FileUploadDto>(); // Vazio

        var act = async () => await _sut.ExecuteAsync(request, documentos);

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("É obrigatório enviar pelo menos um documento (ex: Contrato Social ou Identidade).");
    }

    [Fact]
    public async Task ExecuteAsync_DeveLancarExcecao_QuandoEmpresaEstiverComStatusInvalido()
    {
        var request = new CompleteRegistrationRequestDto { EmpresaId = Guid.NewGuid() };
        var documentos = new List<FileUploadDto> { new FileUploadDto("doc.pdf", Stream.Null, "application/pdf") };
        
        var empresa = new Empresa(new Cnpj("18045008000150")); // Status inicial: AguardandoConfirmacaoEmail
        
        _empresaRepositoryMock.GetByIdAsync(request.EmpresaId).Returns(empresa);

        var act = async () => await _sut.ExecuteAsync(request, documentos);

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("A empresa não está na etapa de completar cadastro.");
    }

    [Fact]
    public async Task ExecuteAsync_DeveCompletarCadastroEAtivarTenant_QuandoDadosForemValidos()
    {
        var request = new CompleteRegistrationRequestDto 
        { 
            EmpresaId = Guid.NewGuid(),
            RazaoSocial = "Nova Razão Social",
            NomeFantasia = "Fantasia",
            Cep = "01001000",
            Logradouro = "Praça da Sé",
            Numero = "1",
            Bairro = "Sé",
            Municipio = "São Paulo",
            Uf = "SP",
            Telefone = "11999999999"
        };
        var documentos = new List<FileUploadDto> { new FileUploadDto("doc.pdf", Stream.Null, "application/pdf") };
        
        var empresa = new Empresa(new Cnpj("18045008000150"));
        empresa.AguardarDadosCompletos(); // Coloca no status correto para passar pela validação
        
        _empresaRepositoryMock.GetByIdAsync(request.EmpresaId).Returns(empresa);
        _storageServiceMock.UploadArquivoAsync(Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<string>())
            .Returns("minierp-documentos/123-doc.pdf");
        _unitOfWorkMock.CommitAsync().Returns(true);

        var result = await _sut.ExecuteAsync(request, documentos);

        result.Should().BeTrue();
        empresa.Status.Should().Be(TenantStatus.Ativo);
        empresa.RazaoSocial.Should().Be("Nova Razão Social");
        empresa.Documentos.Should().HaveCount(1);
        
        await _storageServiceMock.Received(1).UploadArquivoAsync("doc.pdf", Stream.Null, "application/pdf");
        await _unitOfWorkMock.Received(1).CommitAsync();
    }
}