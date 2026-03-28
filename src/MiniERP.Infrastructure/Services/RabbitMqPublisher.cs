using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using MiniERP.Domain.Interfaces;
using RabbitMQ.Client;

namespace MiniERP.Infrastructure.Services;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly IConfiguration _configuration;

    public RabbitMqPublisher(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task PublicarAsync<T>(T mensagem, string fila)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMqSettings:HostName"],
            UserName = _configuration["RabbitMqSettings:UserName"],
            Password = _configuration["RabbitMqSettings:Password"]
        };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: fila, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var json = JsonSerializer.Serialize(mensagem);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: fila, body: body);
    }
}