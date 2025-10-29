using System;
using System.Collections.Generic;

namespace Readioo.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    public string GenreName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<AuthorGenre> AuthorGenres { get; set; } = new List<AuthorGenre>();

    public virtual ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();

    public virtual ICollection<UserGenre> UserGenres { get; set; } = new List<UserGenre>();
}
