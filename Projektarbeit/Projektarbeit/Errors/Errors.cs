namespace Projektarbeit.Errors;

public static class Errors
{
    public static readonly Error WrongPassword = new("WrongPassword", "The password does not match the user");
    public static readonly Error EmailAlreadyExists = new("EmailAlreadyExists", "Another user with this email already exists");
    public static readonly Error NoAuth = new("NoAuth", "No authentication provided");
    public static readonly Error BookingOnlyForYourself = new("BookingOnlyForYourself", "You cannot create a booking for someone else");
}
public record Error(string ErrorCode, string ErrorMessage);