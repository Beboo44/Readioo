

using Readioo.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Readioo.Models;

public partial class Book: BaseEntity
{

    public string Title { get; set; } = null!;

    public string Isbn { get; set; } = null!;

    public string Language { get; set; } = null!;

    public int AuthorId { get; set; }

    public int PagesCount { get; set; }

    public DateOnly PublishDate { get; set; }

    public string? MainCharacters { get; set; }

    public decimal Rate { get; set; }

    public string Description { get; set; } = null!;

    public string? BookImage { get; set; }

    [NotMapped]
    public bool IsAdmin { get; set; } = false;

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();

    public virtual ICollection<BookShelf> BookShelves { get; set; } = new List<BookShelf>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();
}
