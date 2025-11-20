using Readioo.Data.Models;
using System;
using System.Collections.Generic;

namespace Readioo.Models;

public partial class BookShelf:BaseEntity
{

    public int BookId { get; set; }

    public int ShelfId { get; set; }

    public int Status { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Shelf Shelf { get; set; } = null!;
}
