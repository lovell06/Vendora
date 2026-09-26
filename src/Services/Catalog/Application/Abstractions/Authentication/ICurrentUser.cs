namespace Vendora.Services.Catalog.Application.Abstractions.Authentication;

public interface ICurrentUser
{
    Guid UserId { get; }
    Task<string> GetAccessTokenAsync();
}