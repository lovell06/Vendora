namespace Vendora.Services.Identity.Application.Abstractions.Authentication;

public interface IPasswordHashProvider
{
    public string Hash(string passwordRaw);
    public bool Verify(string passwordHash, string passwordRaw);
}