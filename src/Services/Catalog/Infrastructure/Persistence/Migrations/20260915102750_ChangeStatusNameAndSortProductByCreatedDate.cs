using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendora.Services.Catalog.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStatusNameAndSortProductByCreatedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "products",
                newName: "status");

            migrationBuilder.CreateIndex(
                name: "IX_products_created_at_id",
                table: "products",
                columns: new[] { "created_at", "id" },
                unique: true,
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_products_created_at_id",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "products",
                newName: "Status");
        }
    }
}
