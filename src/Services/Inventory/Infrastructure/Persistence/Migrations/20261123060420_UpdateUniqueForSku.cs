using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendora.Services.Inventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUniqueForSku : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_inventory_items_sku",
                table: "inventory_items",
                column: "sku",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_inventory_items_sku",
                table: "inventory_items");
        }
    }
}
