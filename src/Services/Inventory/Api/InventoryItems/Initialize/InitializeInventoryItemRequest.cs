using Vendora.Services.Inventory.Application.InventoryItems.Initialize;

namespace Vendora.Services.Inventory.Api.InventoryItems.Initialize;

public sealed class InitializeInventoryItemRequest
{
    public const string Pattern = "/initialize";

    public int ProductId { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            ProductId = ProductId,
        };
    }
}