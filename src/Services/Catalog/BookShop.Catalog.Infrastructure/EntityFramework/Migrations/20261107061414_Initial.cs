using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Catalog.Infrastructure.EntityFramework.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "catalog");

        migrationBuilder.CreateTable(
            name: "authors",
            schema: "catalog",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_authors", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "book_edition",
            schema: "catalog",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                isbn = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: false),
                publisher_id = table.Column<Guid>(type: "uuid", nullable: false),
                format = table.Column<int>(type: "integer", nullable: false),
                language = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                publication_date = table.Column<DateOnly>(type: "date", nullable: false),
                page_count = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_book_edition", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "books",
            schema: "catalog",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                status = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_books", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "genre",
            schema: "catalog",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                parent_genre_id = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_genre", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "publisher",
            schema: "catalog",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_publisher", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "book_author",
            schema: "catalog",
            columns: table => new
            {
                book_id = table.Column<Guid>(type: "uuid", nullable: false),
                author_id = table.Column<Guid>(type: "uuid", nullable: false),
                order = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_book_author", x => new { x.book_id, x.author_id });
                table.ForeignKey(
                    name: "fk_book_author_authors_author_id",
                    column: x => x.author_id,
                    principalSchema: "catalog",
                    principalTable: "authors",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_book_author_books_book_id",
                    column: x => x.book_id,
                    principalSchema: "catalog",
                    principalTable: "books",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "book_genre",
            schema: "catalog",
            columns: table => new
            {
                book_id = table.Column<Guid>(type: "uuid", nullable: false),
                genres_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_book_genre", x => new { x.book_id, x.genres_id });
                table.ForeignKey(
                    name: "fk_book_genre_books_book_id",
                    column: x => x.book_id,
                    principalSchema: "catalog",
                    principalTable: "books",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_book_genre_genre_genres_id",
                    column: x => x.genres_id,
                    principalSchema: "catalog",
                    principalTable: "genre",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_book_author_author_id",
            schema: "catalog",
            table: "book_author",
            column: "author_id");

        migrationBuilder.CreateIndex(
            name: "ix_book_genre_genres_id",
            schema: "catalog",
            table: "book_genre",
            column: "genres_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "book_author",
            schema: "catalog");

        migrationBuilder.DropTable(
            name: "book_edition",
            schema: "catalog");

        migrationBuilder.DropTable(
            name: "book_genre",
            schema: "catalog");

        migrationBuilder.DropTable(
            name: "publisher",
            schema: "catalog");

        migrationBuilder.DropTable(
            name: "authors",
            schema: "catalog");

        migrationBuilder.DropTable(
            name: "books",
            schema: "catalog");

        migrationBuilder.DropTable(
            name: "genre",
            schema: "catalog");
    }
}
