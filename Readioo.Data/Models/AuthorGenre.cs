using System;
using System.Collections.Generic;

namespace Readioo.Models;

public partial class AuthorGenre
{
    public int AuthorGenreId { get; set; }

    public int AuthorId { get; set; }

    public int GenreId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;
}
