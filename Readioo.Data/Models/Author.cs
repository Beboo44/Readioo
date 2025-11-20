

using Readioo.Data.Models;
using System;
using System.Collections.Generic;

namespace Readioo.Models;

public partial class Author:BaseEntity
{

    public string FullName { get; set; }

    public string Bio { get; set; } = null!;

    public string BirthCountry { get; set; } = null!;

    public string BirthCity { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public DateOnly? DeathDate { get; set; }

    public string? AuthorImage { get; set; }

    public virtual ICollection<AuthorGenre> AuthorGenres { get; set; } = new List<AuthorGenre>();

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
