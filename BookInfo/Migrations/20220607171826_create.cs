using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookInfo.Migrations
{
    public partial class create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BooksGenres",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksGenres", x => new { x.GenreId, x.BookId });
                });

            migrationBuilder.CreateTable(
                name: "FavoriteBooks",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteBooks", x => new { x.UserId, x.BookId });
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BooksGenreBookId = table.Column<int>(type: "int", nullable: true),
                    BooksGenreGenreId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Genres_BooksGenres_BooksGenreGenreId_BooksGenreBookId",
                        columns: x => new { x.BooksGenreGenreId, x.BooksGenreBookId },
                        principalTable: "BooksGenres",
                        principalColumns: new[] { "GenreId", "BookId" });
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pages = table.Column<long>(type: "bigint", nullable: false),
                    Cover = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<int>(type: "int", nullable: true),
                    BooksGenreBookId = table.Column<int>(type: "int", nullable: true),
                    BooksGenreGenreId = table.Column<int>(type: "int", nullable: true),
                    FavoriteBooksBookId = table.Column<int>(type: "int", nullable: true),
                    FavoriteBooksUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Books_BooksGenres_BooksGenreGenreId_BooksGenreBookId",
                        columns: x => new { x.BooksGenreGenreId, x.BooksGenreBookId },
                        principalTable: "BooksGenres",
                        principalColumns: new[] { "GenreId", "BookId" });
                    table.ForeignKey(
                        name: "FK_Books_FavoriteBooks_FavoriteBooksUserId_FavoriteBooksBookId",
                        columns: x => new { x.FavoriteBooksUserId, x.FavoriteBooksBookId },
                        principalTable: "FavoriteBooks",
                        principalColumns: new[] { "UserId", "BookId" });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    FavoriteBooksBookId = table.Column<int>(type: "int", nullable: true),
                    FavoriteBooksUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_FavoriteBooks_FavoriteBooksUserId_FavoriteBooksBookId",
                        columns: x => new { x.FavoriteBooksUserId, x.FavoriteBooksBookId },
                        principalTable: "FavoriteBooks",
                        principalColumns: new[] { "UserId", "BookId" });
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "admin" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "user" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FavoriteBooksBookId", "FavoriteBooksUserId", "Password", "RoleId" },
                values: new object[] { 1, "admin@mail.ru", null, null, "123456", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_BooksGenreGenreId_BooksGenreBookId",
                table: "Books",
                columns: new[] { "BooksGenreGenreId", "BooksGenreBookId" });

            migrationBuilder.CreateIndex(
                name: "IX_Books_FavoriteBooksUserId_FavoriteBooksBookId",
                table: "Books",
                columns: new[] { "FavoriteBooksUserId", "FavoriteBooksBookId" });

            migrationBuilder.CreateIndex(
                name: "IX_Genres_BooksGenreGenreId_BooksGenreBookId",
                table: "Genres",
                columns: new[] { "BooksGenreGenreId", "BooksGenreBookId" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_FavoriteBooksUserId_FavoriteBooksBookId",
                table: "Users",
                columns: new[] { "FavoriteBooksUserId", "FavoriteBooksBookId" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "BooksGenres");

            migrationBuilder.DropTable(
                name: "FavoriteBooks");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
