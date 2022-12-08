namespace Projektarbeit.Errors;

public class BadRequestException : Exception
{
    
    public List<Error> Errors { get; }

    public BadRequestException(Error error)
    {
        Errors = new List<Error> { error };
    }
    
    public BadRequestException(List<Error> errors)
    {
        Errors = errors;
    }
}