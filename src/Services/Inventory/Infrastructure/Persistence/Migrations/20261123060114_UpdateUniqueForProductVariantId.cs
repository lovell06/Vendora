using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendora.Services.Inventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUniqueForProductVariantId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_inventory_items_product_variant_id",
                table: "inventory_items",
                column: "product_variant_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_inventory_items_product_variant_id",
                table: "inventory_items");
        }
    }
}
