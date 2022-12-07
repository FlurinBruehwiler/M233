namespace Projektarbeit.Errors;

public static class Errors
{
    public static readonly Error WrongPassword = new("WrongPassword", "The password does not match the user");
    public static readonly Error UsernameAlreadyExists = new("UsernameExists", "Another user with this username already exists");
    public static readonly Error NoAuth = new("NoAuth", "No authentication provided");
    public static readonly Error UserNotFound = new("UserNotFound", "No User with this username found");
    public static readonly Error PasswordEmpty = new("PasswordEmpty", "Password cant be empty");
    public static readonly Error UsernameEmpty = new("UsernameEmpty", "Username cant be empty");
    public static readonly Error EntryNotFound = new("EntryNotFound", "The Entry was not found");
    public static readonly Error UserHasNoRights = new("UserHasNoRights", "The User has no rights to access this object");
    public static readonly Error CategoryNotFound = new("CategoryNotFound", "The Category was not found");
    public static readonly Error TagNotFound = new("TagNotFound", "The Tag was not found");
    public static readonly Error BookingNotFound = new("BookingNotFound", "The Booking was not found");
    public static readonly Error BookingOnlyForYourself = new("BookingOnlyForYourself", "You cannot create a booking for someone else");
}
public record Error(string ErrorCode, string ErrorMessage);