using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendora.Services.Identity.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddErrorColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "error",
                table: "outbox_messages",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "error",
                table: "outbox_messages");
        }
    }
}
