namespace Vendora.Services.Cart.Application.Abstractions.Authentication;

public interface ICurrentUser
{
    Guid UserId { get; }
    Task<string> GetAccessTokenAsync();
}