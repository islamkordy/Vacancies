namespace Domain;

public class Constants
{
    public const string recipient_number_regex = @"^01[0-2]\d{1,8}$";
    public const string password_regex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\d!@#?[\]])[^\s<>]{8,}$";
}
