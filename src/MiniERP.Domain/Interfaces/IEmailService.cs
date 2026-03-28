namespace MiniERP.Domain.Interfaces;

public interface IEmailService
{
    Task EnviarEmailAsync(string para, string assunto, string corpoHtml);
}