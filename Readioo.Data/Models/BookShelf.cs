using System;
using System.Collections.Generic;

namespace Readioo.Models;

public partial class BookShelf
{
    public int BookShelfId { get; set; }

    public int BookId { get; set; }

    public int ShelfId { get; set; }

    public int Status { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Shelf Shelf { get; set; } = null!;
}
