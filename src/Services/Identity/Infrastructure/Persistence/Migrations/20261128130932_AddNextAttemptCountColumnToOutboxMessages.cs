using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendora.Services.Identity.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNextAttemptCountColumnToOutboxMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "retry_count",
                table: "outbox_messages",
                newName: "attempt_count");

            migrationBuilder.RenameColumn(
                name: "error",
                table: "outbox_messages",
                newName: "last_error");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "next_attempt_at",
                table: "outbox_messages",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "next_attempt_at",
                table: "outbox_messages");

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
