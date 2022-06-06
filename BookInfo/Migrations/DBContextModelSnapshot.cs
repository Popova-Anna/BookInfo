﻿// <auto-generated />
using System;
using BookInfo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookInfo.Migrations
{
    [DbContext(typeof(DBContext))]
    partial class DBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BookInfo.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("BookInfo.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int?>("BooksGenreIdBook")
                        .HasColumnType("int");

                    b.Property<int?>("BooksGenreIdGenre")
                        .HasColumnType("int");

                    b.Property<string>("Cover")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FavoriteBooksIdBook")
                        .HasColumnType("int");

                    b.Property<int?>("FavoriteBooksIdUser")
                        .HasColumnType("int");

                    b.Property<int?>("IdAuthor")
                        .HasColumnType("int");

                    b.Property<long>("Pages")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BooksGenreIdGenre", "BooksGenreIdBook");

                    b.HasIndex("FavoriteBooksIdUser", "FavoriteBooksIdBook");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("BookInfo.Models.BooksGenre", b =>
                {
                    b.Property<int>("IdGenre")
                        .HasColumnType("int")
                        .HasColumnOrder(2);

                    b.Property<int>("IdBook")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.HasKey("IdGenre", "IdBook");

                    b.ToTable("BooksGenres");
                });

            modelBuilder.Entity("BookInfo.Models.FavoriteBooks", b =>
                {
                    b.Property<int>("IdUser")
                        .HasColumnType("int")
                        .HasColumnOrder(2);

                    b.Property<int>("IdBook")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.HasKey("IdUser", "IdBook");

                    b.ToTable("FavoriteBooks");
                });

            modelBuilder.Entity("BookInfo.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("BooksGenreIdBook")
                        .HasColumnType("int");

                    b.Property<int?>("BooksGenreIdGenre")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BooksGenreIdGenre", "BooksGenreIdBook");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("BookInfo.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "admin"
                        },
                        new
                        {
                            Id = 2,
                            Name = "user"
                        });
                });

            modelBuilder.Entity("BookInfo.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FavoriteBooksIdBook")
                        .HasColumnType("int");

                    b.Property<int?>("FavoriteBooksIdUser")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("FavoriteBooksIdUser", "FavoriteBooksIdBook");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@mail.ru",
                            Password = "123456",
                            RoleId = 1
                        });
                });

            modelBuilder.Entity("BookInfo.Models.Book", b =>
                {
                    b.HasOne("BookInfo.Models.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId");

                    b.HasOne("BookInfo.Models.BooksGenre", null)
                        .WithMany("Books")
                        .HasForeignKey("BooksGenreIdGenre", "BooksGenreIdBook");

                    b.HasOne("BookInfo.Models.FavoriteBooks", null)
                        .WithMany("Books")
                        .HasForeignKey("FavoriteBooksIdUser", "FavoriteBooksIdBook");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("BookInfo.Models.Genre", b =>
                {
                    b.HasOne("BookInfo.Models.BooksGenre", null)
                        .WithMany("Genres")
                        .HasForeignKey("BooksGenreIdGenre", "BooksGenreIdBook");
                });

            modelBuilder.Entity("BookInfo.Models.User", b =>
                {
                    b.HasOne("BookInfo.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");

                    b.HasOne("BookInfo.Models.FavoriteBooks", null)
                        .WithMany("Users")
                        .HasForeignKey("FavoriteBooksIdUser", "FavoriteBooksIdBook");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("BookInfo.Models.Author", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("BookInfo.Models.BooksGenre", b =>
                {
                    b.Navigation("Books");

                    b.Navigation("Genres");
                });

            modelBuilder.Entity("BookInfo.Models.FavoriteBooks", b =>
                {
                    b.Navigation("Books");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("BookInfo.Models.Role", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
