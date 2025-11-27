using Readioo.Data.Models;
using System;
using System.Collections.Generic;

namespace Readioo.Models;

public partial class UserBook:BaseEntity
{
    public int UserId { get; set; }
    public int BookId { get; set; }
    public int? UserRating { get; set; } // Add this field to store user's rating (1-5)
    public virtual Book Book { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
