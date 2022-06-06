using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookInfo.Migrations
{
    public partial class cretdbAll : Migration
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
                    IdBook = table.Column<int>(type: "int", nullable: false),
                    IdGenre = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksGenres", x => new { x.IdGenre, x.IdBook });
                });

            migrationBuilder.CreateTable(
                name: "FavoriteBooks",
                columns: table => new
                {
                    IdBook = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteBooks", x => new { x.IdUser, x.IdBook });
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
                    BooksGenreIdBook = table.Column<int>(type: "int", nullable: true),
                    BooksGenreIdGenre = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Genres_BooksGenres_BooksGenreIdGenre_BooksGenreIdBook",
                        columns: x => new { x.BooksGenreIdGenre, x.BooksGenreIdBook },
                        principalTable: "BooksGenres",
                        principalColumns: new[] { "IdGenre", "IdBook" });
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
                    IdAuthor = table.Column<int>(type: "int", nullable: true),
                    AuthorId = table.Column<int>(type: "int", nullable: true),
                    BooksGenreIdBook = table.Column<int>(type: "int", nullable: true),
                    BooksGenreIdGenre = table.Column<int>(type: "int", nullable: true),
                    FavoriteBooksIdBook = table.Column<int>(type: "int", nullable: true),
                    FavoriteBooksIdUser = table.Column<int>(type: "int", nullable: true)
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
                        name: "FK_Books_BooksGenres_BooksGenreIdGenre_BooksGenreIdBook",
                        columns: x => new { x.BooksGenreIdGenre, x.BooksGenreIdBook },
                        principalTable: "BooksGenres",
                        principalColumns: new[] { "IdGenre", "IdBook" });
                    table.ForeignKey(
                        name: "FK_Books_FavoriteBooks_FavoriteBooksIdUser_FavoriteBooksIdBook",
                        columns: x => new { x.FavoriteBooksIdUser, x.FavoriteBooksIdBook },
                        principalTable: "FavoriteBooks",
                        principalColumns: new[] { "IdUser", "IdBook" });
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
                    FavoriteBooksIdBook = table.Column<int>(type: "int", nullable: true),
                    FavoriteBooksIdUser = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_FavoriteBooks_FavoriteBooksIdUser_FavoriteBooksIdBook",
                        columns: x => new { x.FavoriteBooksIdUser, x.FavoriteBooksIdBook },
                        principalTable: "FavoriteBooks",
                        principalColumns: new[] { "IdUser", "IdBook" });
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
                columns: new[] { "Id", "Email", "FavoriteBooksIdBook", "FavoriteBooksIdUser", "Password", "RoleId" },
                values: new object[] { 1, "admin@mail.ru", null, null, "123456", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_BooksGenreIdGenre_BooksGenreIdBook",
                table: "Books",
                columns: new[] { "BooksGenreIdGenre", "BooksGenreIdBook" });

            migrationBuilder.CreateIndex(
                name: "IX_Books_FavoriteBooksIdUser_FavoriteBooksIdBook",
                table: "Books",
                columns: new[] { "FavoriteBooksIdUser", "FavoriteBooksIdBook" });

            migrationBuilder.CreateIndex(
                name: "IX_Genres_BooksGenreIdGenre_BooksGenreIdBook",
                table: "Genres",
                columns: new[] { "BooksGenreIdGenre", "BooksGenreIdBook" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_FavoriteBooksIdUser_FavoriteBooksIdBook",
                table: "Users",
                columns: new[] { "FavoriteBooksIdUser", "FavoriteBooksIdBook" });

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
