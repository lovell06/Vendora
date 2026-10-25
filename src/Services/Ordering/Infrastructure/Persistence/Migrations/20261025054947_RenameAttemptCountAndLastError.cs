using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendora.Services.Ordering.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameAttemptCountAndLastError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "error",
                table: "outbox_messages",
                newName: "last_error");

            migrationBuilder.RenameColumn(
                name: "retry_count",
                table: "outbox_messages",
                newName: "attempt_count");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "last_error",
                table: "outbox_messages",
                newName: "error");

            migrationBuilder.RenameColumn(
                name: "attempt_count",
                table: "outbox_messages",
                newName: "retry_count");
        }
    }
}
