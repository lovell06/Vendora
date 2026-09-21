using Vendora.Services.Inventory.Application.InventoryItems.Create;

namespace Vendora.Services.Inventory.Api.InventoryItems.Create;

public sealed class CreateInventoryItemRequest
{
    public const string Pattern = "/create";

    public int ProductId { get; init; }
    public int OnHandQuantity { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            ProductId = ProductId,
            OnHandQuantity = OnHandQuantity
        };
    }
}