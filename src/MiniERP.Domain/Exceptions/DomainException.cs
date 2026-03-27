namespace MiniERP.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    { }

    public static void When(bool hasError, string error)
    {
        if (hasError)
            throw new DomainException(error);
    }
}