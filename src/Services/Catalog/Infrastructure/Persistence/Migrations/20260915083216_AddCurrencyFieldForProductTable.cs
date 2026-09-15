using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendora.Services.Catalog.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyFieldForProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "currency",
                table: "products",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currency",
                table: "products");
        }
    }
}
