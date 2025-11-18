using Readioo.Data.Models;
using System;
using System.Collections.Generic;

namespace Readioo.Models;

public partial class Shelf: BaseEntity
{
    public int ShelfId { get; set; }

    public string ShelfName { get; set; } = null!;

    public int UserId { get; set; }

    public virtual ICollection<BookShelf> BookShelves { get; set; } = new List<BookShelf>();

    public virtual User User { get; set; } = null!;
}
