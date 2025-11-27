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
        base.OnModelCreating(modelBuilder);

        // --- Existing Configurations ---

        // Configure BookGenre composite key
        modelBuilder.Entity<BookGenre>()
            .HasKey(bg => new { bg.BookId, bg.GenreId });

        // Configure Book -> BookGenre relationship
        modelBuilder.Entity<BookGenre>()
            .HasOne(bg => bg.Book)
            .WithMany(b => b.BookGenres)
            .HasForeignKey(bg => bg.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Genre -> BookGenre relationship
        modelBuilder.Entity<BookGenre>()
            .HasOne(bg => bg.Genre)
            .WithMany(g => g.BookGenres)
            .HasForeignKey(bg => bg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookShelf>()
        .HasKey(bg => new { bg.BookId, bg.ShelfId });

        modelBuilder.Entity<BookShelf>()
            .HasOne(bg => bg.Book)
            .WithMany(b => b.BookShelves)
            .HasForeignKey(bg => bg.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookShelf>()
            .HasOne(bg => bg.Shelf)
            .WithMany(g => g.BookShelves)
            .HasForeignKey(bg => bg.ShelfId)
            .OnDelete(DeleteBehavior.Cascade);



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

        // SEEDING DATA STARTS HERE
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, GenreName = "Science Fiction", Description = "Explore the limitless possibilities of the universe. From space operas and alien civilizations to cybernetic futures and time travel, these stories ask the ultimate 'what if' questions about technology and humanity." },
            new Genre { Id = 2, GenreName = "Horror", Description = "Prepare to be terrified. These spine-chilling tales explore the darker side of existence, featuring supernatural entities, psychological torments, and the unknown, designed to keep you on the edge of your seat and awake at night." },
            new Genre { Id = 3, GenreName = "Dystopian", Description = "Enter worlds where society has collapsed or turned oppressive. These gripping narratives explore survival, rebellion, and the resilience of the human spirit amidst totalitarian regimes, environmental disasters, or post-apocalyptic ruins." },
            new Genre { Id = 4, GenreName = "Fantasy", Description = "Escape into realms of magic, myth, and adventure. Whether it’s epic quests to save a kingdom, battles with dragons, or subtle magical realism, these stories weave folklore and imagination into unforgettable journeys." },
            new Genre { Id = 5, GenreName = "Thriller", Description = "Fast-paced and adrenaline-fueled, these books are impossible to put down. Packed with high stakes, dangerous chases, and shocking plot twists, thrillers keep you guessing untill the very last page." },
            new Genre { Id = 6, GenreName = "Mystery", Description = "Put on your detective hat and solve the puzzle. From classic whodunits and noir investigations to cozy village secrets, these stories revolve around crime, clues, and the thrill of uncovering the truth." },
            new Genre { Id = 7, GenreName = "Drama", Description = "Dive deep into the complexities of the human experience. These emotionally resonant stories focus on realistic characters, interpersonal conflicts, and the triumphs and tragedies of everyday life." },
            new Genre { Id = 8, GenreName = "Historical", Description = "Travel back in time to experience different eras. Meticulously researched and atmospherically rich, these novels transport readers to the past, blending real events with fictional lives to bring history to life." },
            new Genre { Id = 9, GenreName = "Political", Description = "Explore the corridors of power and the mechanisms of governance. These narratives critique societal structures, expose corruption, and delve into the ideologies that shape nations and revolutions." },
            new Genre { Id = 10, GenreName = "Novel", Description = "Immerse yourself in the art of long-form storytelling. These works of fiction prioritize character development and narrative depth, offering a profound exploration of themes that resonate with the human condition." },
            new Genre { Id = 11, GenreName = "Biography", Description = "Discover the true stories behind the names. These accounts offer intimate looks into the lives of historical figures, innovators, and cultural icons, providing inspiration and insight through their struggles and achievements." },
            new Genre { Id = 12, GenreName = "Philosophy", Description = "Challenge your perspective and expand your mind. These works delve into the fundamental nature of knowledge, reality, ethics, and existence, encouraging deep contemplation on the meaning of life." },
            new Genre { Id = 13, GenreName = "Crime", Description = "Step into the shadowy world of lawbreakers and law enforcement. Focusing on the perpetrators, the victims, and the investigators, these gritty tales explore the motives behind criminal acts and the quest for justice." },
            new Genre { Id = 14, GenreName = "Short Stories", Description = "Perfect for quick escapes, these collections offer bite-sized narratives that pack a punch. Ranging from experimental vignettes to fully realized tales, they capture distinct moments and emotions in a concise format." },
            new Genre { Id = 15, GenreName = "Satire", Description = "A sharp wit meets social commentary. Using humor, irony, and exaggeration, these clever stories expose the absurdities of society, politics, and human behavior, making you think while you laugh." }
        );

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}