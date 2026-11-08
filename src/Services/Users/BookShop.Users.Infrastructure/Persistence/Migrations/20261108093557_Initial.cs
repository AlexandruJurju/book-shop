using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookShop.Users.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "users");

        migrationBuilder.CreateTable(
            name: "inbox_message_consumers",
            schema: "users",
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
            schema: "users",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                type = table.Column<string>(type: "text", nullable: false),
                content = table.Column<string>(type: "jsonb", maxLength: 2000, nullable: false),
                occurred_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                error = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_inbox_messages", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "outbox_message_consumers",
            schema: "users",
            columns: table => new
            {
                outbox_message_id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_outbox_message_consumers", x => new { x.outbox_message_id, x.name });
            });

        migrationBuilder.CreateTable(
            name: "outbox_messages",
            schema: "users",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                type = table.Column<string>(type: "text", nullable: false),
                content = table.Column<string>(type: "jsonb", maxLength: 2000, nullable: false),
                occurred_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                error = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_outbox_messages", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "permissions",
            schema: "users",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_permissions", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "roles",
            schema: "users",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_roles", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "users",
            schema: "users",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                user_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                identity_id = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_users", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "permission_role",
            schema: "users",
            columns: table => new
            {
                permissions_id = table.Column<int>(type: "integer", nullable: false),
                role_id = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_permission_role", x => new { x.permissions_id, x.role_id });
                table.ForeignKey(
                    name: "fk_permission_role_permissions_permissions_id",
                    column: x => x.permissions_id,
                    principalSchema: "users",
                    principalTable: "permissions",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_permission_role_roles_role_id",
                    column: x => x.role_id,
                    principalSchema: "users",
                    principalTable: "roles",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "role_user",
            schema: "users",
            columns: table => new
            {
                roles_id = table.Column<int>(type: "integer", nullable: false),
                users_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_role_user", x => new { x.roles_id, x.users_id });
                table.ForeignKey(
                    name: "fk_role_user_roles_roles_id",
                    column: x => x.roles_id,
                    principalSchema: "users",
                    principalTable: "roles",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_role_user_users_users_id",
                    column: x => x.users_id,
                    principalSchema: "users",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            schema: "users",
            table: "permissions",
            columns: ["id", "name"],
            values: new object[,]
            {
                { 1, "users:read" },
                { 2, "users:update" }
            });

        migrationBuilder.InsertData(
            schema: "users",
            table: "roles",
            columns: ["id", "name"],
            values: new object[,]
            {
                { 1, "Administrator" },
                { 2, "Member" }
            });

        migrationBuilder.CreateIndex(
                name: "idx_inbox_messages_unprocessed",
                schema: "users",
                table: "inbox_messages",
                columns: ["occurred_on_utc", "processed_on_utc"],
                filter: "\"processed_on_utc\" IS NULL")
            .Annotation("Npgsql:IndexInclude", (string[])["id", "type", "content"]);

        migrationBuilder.CreateIndex(
                name: "idx_outbox_messages_unprocessed",
                schema: "users",
                table: "outbox_messages",
                columns: ["occurred_on_utc", "processed_on_utc"],
                filter: "\"processed_on_utc\" IS NULL")
            .Annotation("Npgsql:IndexInclude", (string[])["id", "type", "content"]);

        migrationBuilder.CreateIndex(
            name: "ix_permission_role_role_id",
            schema: "users",
            table: "permission_role",
            column: "role_id");

        migrationBuilder.CreateIndex(
            name: "ix_role_user_users_id",
            schema: "users",
            table: "role_user",
            column: "users_id");

        migrationBuilder.CreateIndex(
            name: "ix_users_email",
            schema: "users",
            table: "users",
            column: "email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_users_identity_id",
            schema: "users",
            table: "users",
            column: "identity_id",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "inbox_message_consumers",
            schema: "users");

        migrationBuilder.DropTable(
            name: "inbox_messages",
            schema: "users");

        migrationBuilder.DropTable(
            name: "outbox_message_consumers",
            schema: "users");

        migrationBuilder.DropTable(
            name: "outbox_messages",
            schema: "users");

        migrationBuilder.DropTable(
            name: "permission_role",
            schema: "users");

        migrationBuilder.DropTable(
            name: "role_user",
            schema: "users");

        migrationBuilder.DropTable(
            name: "permissions",
            schema: "users");

        migrationBuilder.DropTable(
            name: "roles",
            schema: "users");

        migrationBuilder.DropTable(
            name: "users",
            schema: "users");
    }
}
