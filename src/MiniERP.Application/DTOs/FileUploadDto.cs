namespace MiniERP.Application.DTOs;

public record FileUploadDto(string NomeArquivo, Stream Conteudo, string ContentType);