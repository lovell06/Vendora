using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendora.Services.Catalog.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SwitchSizeBytesToBigIntegerInProductImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "size_bytes",
                table: "product_images",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "size_bytes",
                table: "product_images",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
