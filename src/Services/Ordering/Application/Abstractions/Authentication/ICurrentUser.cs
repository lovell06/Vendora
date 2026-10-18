namespace Vendora.Services.Ordering.Application.Abstractions.Authentication;

public interface ICurrentUser
{
    Guid UserId { get; }

    Task<string> GetAccessTokenAsync();
}