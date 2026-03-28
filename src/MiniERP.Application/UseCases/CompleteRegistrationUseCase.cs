using MiniERP.Application.DTOs;
using MiniERP.Domain.Entities;
using MiniERP.Domain.Enums;
using MiniERP.Domain.Exceptions;
using MiniERP.Domain.Interfaces;
using MiniERP.Domain.ValueObjects;

namespace MiniERP.Application.UseCases;

public class CompleteRegistrationUseCase
{
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteRegistrationUseCase(IEmpresaRepository empresaRepository, IStorageService storageService, IUnitOfWork unitOfWork)
    {
        _empresaRepository = empresaRepository;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(CompleteRegistrationRequestDto request, IEnumerable<FileUploadDto> documentosUpload)
    {
        var documentosList = documentosUpload.ToList();
        DomainException.When(!documentosList.Any(), "É obrigatório enviar pelo menos um documento (ex: Contrato Social ou Identidade).");

        var empresa = await _empresaRepository.GetByIdAsync(request.EmpresaId);
        DomainException.When(empresa == null, "Empresa não encontrada.");
        DomainException.When(empresa!.Status != TenantStatus.AguardandoDadosCompletos, "A empresa não está na etapa de completar cadastro.");

        empresa.CompletarCadastro(
            request.RazaoSocial, 
            request.NomeFantasia, 
            request.Telefone, 
            new Cep(request.Cep),
            request.Logradouro, 
            request.Numero, 
            request.Complemento, 
            request.Bairro, 
            request.Municipio, 
            request.Uf);


        foreach (var doc in documentosList)
        {
            var s3Path = await _storageService.UploadArquivoAsync(doc.NomeArquivo, doc.Conteudo, doc.ContentType);
            var documentoEmpresa = new DocumentoEmpresa(empresa.Id, doc.NomeArquivo, s3Path);
            empresa.Documentos.Add(documentoEmpresa);
        }

        empresa.AtivarTenant();

        var sucesso = await _unitOfWork.CommitAsync();
        DomainException.When(!sucesso, "Ocorreu um erro ao salvar os dados e os documentos.");

        return true;
    }
}