using Vendora.Services.Cart.Application.Carts.Initialize;

namespace Vendora.Services.Cart.Api.Carts.Initialize;

public sealed class InitializeCartRequest
{
    public const string Pattern = "/initialize";
    public Guid UserId { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            UserId = UserId
        };
    }
}
