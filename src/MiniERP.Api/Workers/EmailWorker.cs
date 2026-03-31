using System.Text;
using System.Text.Json;
using MiniERP.Application.DTOs;
using MiniERP.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MiniERP.Api.Workers;

public class EmailWorker : BackgroundService
{
    private readonly ILogger<EmailWorker> _logger;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    private IConnection? _connection;
    private IChannel? _channel;
    private const string QueueName = "email_queue";

    public EmailWorker(ILogger<EmailWorker> logger, IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _configuration = configuration;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMqSettings:HostName"],
            UserName = _configuration["RabbitMqSettings:UserName"],
            Password = _configuration["RabbitMqSettings:Password"]
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Tentando conectar ao RabbitMQ...");
                _connection = await factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
                await _channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                
                _logger.LogInformation("Conectado ao RabbitMQ com sucesso!");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("RabbitMQ ainda não está pronto. Tentando novamente em 5 segundos... ({Mensagem})", ex.Message);
                await Task.Delay(5000, stoppingToken);
            }
        }

        if (stoppingToken.IsCancellationRequested) return;

        var consumer = new AsyncEventingBasicConsumer(_channel!);
        consumer.ReceivedAsync += async (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            var message = JsonSerializer.Deserialize<EnviarEmailMessage>(content);

            if (message != null)
            {
                _logger.LogInformation("Processando e-mail para: {Email}", message.Para);

                using var scope = _serviceProvider.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                try
                {
                    await emailService.EnviarEmailAsync(message.Para, message.Assunto, message.CorpoHtml);
                    
                    await _channel!.BasicAckAsync(ea.DeliveryTag, false);
                    _logger.LogInformation("E-mail enviado com sucesso.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao enviar e-mail.");
                    await _channel!.BasicNackAsync(ea.DeliveryTag, false, false);
                }
            }
        };

        await _channel!.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}