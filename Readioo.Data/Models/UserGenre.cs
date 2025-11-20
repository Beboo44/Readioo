using Readioo.Data.Models;
using System;
using System.Collections.Generic;

namespace Readioo.Models;

public partial class UserGenre:BaseEntity
{

    public int UserId { get; set; }

    public int GenreId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
