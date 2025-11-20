using Readioo.Data.Models;
using System;
using System.Collections.Generic;

namespace Readioo.Models;

public partial class BookGenre:BaseEntity
{

    public int BookId { get; set; }

    public int GenreId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;
}
