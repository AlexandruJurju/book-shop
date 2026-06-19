using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Users.Infrastructure.EntityFramework.Migrations;

/// <inheritdoc />
public partial class PermissionManyToMany : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_permissions_roles_role_id",
            schema: "users",
            table: "permissions");

        migrationBuilder.DropIndex(
            name: "ix_permissions_role_id",
            schema: "users",
            table: "permissions");

        migrationBuilder.DropColumn(
            name: "role_id",
            schema: "users",
            table: "permissions");

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

        migrationBuilder.CreateIndex(
            name: "ix_permission_role_role_id",
            schema: "users",
            table: "permission_role",
            column: "role_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "permission_role",
            schema: "users");

        migrationBuilder.AddColumn<int>(
            name: "role_id",
            schema: "users",
            table: "permissions",
            type: "integer",
            nullable: true);

        migrationBuilder.UpdateData(
            schema: "users",
            table: "permissions",
            keyColumn: "id",
            keyValue: 1,
            column: "role_id",
            value: null);

        migrationBuilder.UpdateData(
            schema: "users",
            table: "permissions",
            keyColumn: "id",
            keyValue: 2,
            column: "role_id",
            value: null);

        migrationBuilder.CreateIndex(
            name: "ix_permissions_role_id",
            schema: "users",
            table: "permissions",
            column: "role_id");

        migrationBuilder.AddForeignKey(
            name: "fk_permissions_roles_role_id",
            schema: "users",
            table: "permissions",
            column: "role_id",
            principalSchema: "users",
            principalTable: "roles",
            principalColumn: "id");
    }
}
