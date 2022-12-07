namespace Projektarbeit.Errors;

public class BadRequestException : Exception
{
    public Error Error { get; }

    public BadRequestException(Error error)
    {
        Error = error;
    }
}