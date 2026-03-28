using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using MiniERP.Domain.Interfaces;

namespace MiniERP.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task EnviarEmailAsync(string para, string assunto, string corpoHtml)
    {
        var host = _configuration["EmailSettings:SmtpHost"];
        var port = int.Parse(_configuration["EmailSettings:SmtpPort"]!);
        var fromEmail = _configuration["EmailSettings:FromEmail"];
        var fromName = _configuration["EmailSettings:FromName"];

        using var client = new SmtpClient(host, port);
        
        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail!, fromName),
            Subject = assunto,
            Body = corpoHtml,
            IsBodyHtml = true
        };
        
        mailMessage.To.Add(para);

        await client.SendMailAsync(mailMessage);
    }
}