using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Readioo.Models;

namespace Readioo.Data.Data.Contexts;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<AuthorGenre> AuthorGenres { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookGenre> BookGenres { get; set; }

    public virtual DbSet<BookShelf> BookShelves { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Shelf> Shelves { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserBook> UserBooks { get; set; }

    public virtual DbSet<UserGenre> UserGenres { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorGenre>(entity =>
        {
            entity.HasIndex(e => e.AuthorId, "IX_AuthorGenres_AuthorId");

            entity.HasIndex(e => e.GenreId, "IX_AuthorGenres_GenreId");

            entity.HasOne(d => d.Author).WithMany(p => p.AuthorGenres).HasForeignKey(d => d.AuthorId);

            entity.HasOne(d => d.Genre).WithMany(p => p.AuthorGenres).HasForeignKey(d => d.GenreId);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasIndex(e => e.AuthorId, "IX_Books_AuthorId");

            entity.Property(e => e.Isbn).HasColumnName("ISBN");
            entity.Property(e => e.Rate).HasColumnType("decimal(3, 2)");

            entity.HasOne(d => d.Author).WithMany(p => p.Books).HasForeignKey(d => d.AuthorId);
        });

        modelBuilder.Entity<BookGenre>(entity =>
        {
            entity.HasIndex(e => e.BookId, "IX_BookGenres_BookId");

            entity.HasIndex(e => e.GenreId, "IX_BookGenres_GenreId");

            entity.HasOne(d => d.Book).WithMany(p => p.BookGenres).HasForeignKey(d => d.BookId);

            entity.HasOne(d => d.Genre).WithMany(p => p.BookGenres).HasForeignKey(d => d.GenreId);
        });

        modelBuilder.Entity<BookShelf>(entity =>
        {
            entity.HasIndex(e => e.BookId, "IX_BookShelves_BookId");

            entity.HasIndex(e => e.ShelfId, "IX_BookShelves_ShelfId");

            entity.HasOne(d => d.Book).WithMany(p => p.BookShelves).HasForeignKey(d => d.BookId);

            entity.HasOne(d => d.Shelf).WithMany(p => p.BookShelves).HasForeignKey(d => d.ShelfId);
        });

        

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasIndex(e => e.BookId, "IX_Reviews_BookId");

            entity.HasIndex(e => e.UserId, "IX_Reviews_UserId");

            entity.HasOne(d => d.Book).WithMany(p => p.Reviews).HasForeignKey(d => d.BookId);

            entity.HasOne(d => d.User).WithMany(p => p.Reviews).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Shelf>(entity =>
        {
            entity.ToTable("shelves");

            entity.HasIndex(e => e.UserId, "IX_shelves_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.Shelves).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("UserID");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.ProfileUrl).HasColumnName("ProfileURL");
            entity.Property(e => e.UserPassword).HasMaxLength(255);
        });

        modelBuilder.Entity<UserBook>(entity =>
        {
            entity.HasIndex(e => e.BookId, "IX_UserBooks_BookId");

            entity.HasIndex(e => e.UserId, "IX_UserBooks_UserId");

            entity.HasOne(d => d.Book).WithMany(p => p.UserBooks).HasForeignKey(d => d.BookId);

            entity.HasOne(d => d.User).WithMany(p => p.UserBooks).HasForeignKey(d => d.UserId);
        });

        

        modelBuilder.Entity<UserGenre>(entity =>
        {
            entity.HasIndex(e => e.GenreId, "IX_UserGenres_GenreId");

            entity.HasIndex(e => e.UserId, "IX_UserGenres_UserId");

            entity.HasOne(d => d.Genre).WithMany(p => p.UserGenres).HasForeignKey(d => d.GenreId);

            entity.HasOne(d => d.User).WithMany(p => p.UserGenres).HasForeignKey(d => d.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
