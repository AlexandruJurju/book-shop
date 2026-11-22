using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Cart.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class _20261122113703_RemovedMassTransitInbox : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "inbox_message_consumers",
            schema: "cart");

        migrationBuilder.DropTable(
            name: "inbox_messages",
            schema: "cart");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "inbox_message_consumers",
            schema: "cart",
            columns: table => new
            {
                inbox_message_id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_inbox_message_consumers", x => new { x.inbox_message_id, x.name });
            });

        migrationBuilder.CreateTable(
            name: "inbox_messages",
            schema: "cart",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                content = table.Column<string>(type: "jsonb", maxLength: 2000, nullable: false),
                error = table.Column<string>(type: "text", nullable: true),
                occurred_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                type = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_inbox_messages", x => x.id);
            });

        migrationBuilder.CreateIndex(
            name: "idx_inbox_messages_unprocessed",
            schema: "cart",
            table: "inbox_messages",
            columns: ["occurred_on_utc", "processed_on_utc"],
            filter: "\"processed_on_utc\" IS NULL")
            .Annotation("Npgsql:IndexInclude", (string[])["id", "type", "content"]);
    }
}
