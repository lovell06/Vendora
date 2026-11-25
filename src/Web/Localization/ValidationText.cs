namespace Vendora.Web.Localization;

// DataAnnotations read messages through ErrorMessageResourceType/Name, which needs static string properties.
// Each one returns the translation of its English text for the current UI culture.
public static class ValidationText
{
    public static string EnterFullName => Text.Get("Enter your full name.");
    public static string FullNameTooLong => Text.Get("Full name must be 100 characters or fewer.");
    public static string EnterEmail => Text.Get("Enter your email address.");
    public static string InvalidEmail => Text.Get("Enter a valid email address.");
    public static string EnterPhone => Text.Get("Enter your phone number.");
    public static string InvalidPhone => Text.Get("Enter a valid phone number.");
    public static string CreatePassword => Text.Get("Create a password.");
    public static string PasswordTooShort => Text.Get("Password must be at least 8 characters.");
    public static string ConfirmPassword => Text.Get("Confirm your password.");
    public static string PasswordsDoNotMatch => Text.Get("Passwords do not match.");
    public static string EnterPassword => Text.Get("Enter your password.");
}
