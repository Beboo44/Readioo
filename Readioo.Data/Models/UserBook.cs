using Readioo.Data.Models;
using System;
using System.Collections.Generic;

namespace Readioo.Models;

public partial class UserBook:BaseEntity
{
    public int UserId { get; set; }
    public int BookId { get; set; }
    public virtual Book Book { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
