namespace MiniERP.Domain.Interfaces;

public interface IMessagePublisher
{
    Task PublicarAsync<T>(T mensagem, string fila);
}