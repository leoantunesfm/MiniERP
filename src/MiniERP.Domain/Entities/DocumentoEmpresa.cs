namespace MiniERP.Domain.Entities;

public class DocumentoEmpresa
{
    public Guid Id { get; private set; }
    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; } = null!;
    
    public string NomeArquivo { get; private set; } = null!;
    public string S3Path { get; private set; } = null!;
    public DateTime DataUpload { get; private set; }

    protected DocumentoEmpresa() { }

    public DocumentoEmpresa(Guid empresaId, string nomeArquivo, string s3Path)
    {
        Id = Guid.NewGuid();
        EmpresaId = empresaId;
        NomeArquivo = nomeArquivo;
        S3Path = s3Path;
        DataUpload = DateTime.UtcNow;
    }
}