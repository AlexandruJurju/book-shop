using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
#pragma warning disable CA1861

namespace BookShop.Users.Infrastructure.EntityFramework.Migrations;

/// <inheritdoc />
public partial class AddedRolesAndPermissions : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
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
            name: "permissions",
            schema: "users",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                role_id = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_permissions", x => x.id);
                table.ForeignKey(
                    name: "fk_permissions_roles_role_id",
                    column: x => x.role_id,
                    principalSchema: "users",
                    principalTable: "roles",
                    principalColumn: "id");
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
            columns: new[] { "id", "name", "role_id" },
            values: new object[,]
            {
                { 1, "users:read", null },
                { 2, "users:update", null }
            });

        migrationBuilder.InsertData(
            schema: "users",
            table: "roles",
            columns: new[] { "id", "name" },
            values: new object[,]
            {
                { 1, "Administrator" },
                { 2, "Member" }
            });

        migrationBuilder.CreateIndex(
            name: "ix_permissions_role_id",
            schema: "users",
            table: "permissions",
            column: "role_id");

        migrationBuilder.CreateIndex(
            name: "ix_role_user_users_id",
            schema: "users",
            table: "role_user",
            column: "users_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "permissions",
            schema: "users");

        migrationBuilder.DropTable(
            name: "role_user",
            schema: "users");

        migrationBuilder.DropTable(
            name: "roles",
            schema: "users");
    }
}
