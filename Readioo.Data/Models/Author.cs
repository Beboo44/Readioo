using System;
using System.Collections.Generic;

namespace Readioo.Models;

public partial class Author
{
    public int AuthorId { get; set; }

    public int FullName { get; set; }

    public string Bio { get; set; } = null!;

    public string BirthCountry { get; set; } = null!;

    public string BirthCity { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public DateOnly? DeathDate { get; set; }

    public byte[]? AuthorImage { get; set; }

    public virtual ICollection<AuthorGenre> AuthorGenres { get; set; } = new List<AuthorGenre>();

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
