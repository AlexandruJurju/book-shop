using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable IDE0161
#pragma warning disable CA1861
namespace BookShop.Users.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddedIntegrationMessageIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "idx_inbox_messages_unprocessed",
                schema: "users",
                table: "inbox_messages",
                columns: new[] { "occurred_on_utc", "processed_on_utc" },
                filter: "\"processed_on_utc\" IS NULL")
                .Annotation("Npgsql:IndexInclude", new[] { "id", "type", "content" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_inbox_messages_unprocessed",
                schema: "users",
                table: "inbox_messages");
        }
    }
}
#pragma warning restore IDE0161
#pragma warning restore CA1861
