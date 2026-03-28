namespace MiniERP.Domain.Interfaces;

public interface IStorageService
{
    Task<string> UploadArquivoAsync(string nomeArquivo, Stream arquivoStream, string contentType);
}