

using Readioo.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Readioo.Models;

public partial class User: BaseEntity
{
    
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string UserEmail { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public string? Bio { get; set; }

    public byte[]? UserImage { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string? ProfileUrl { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();

    public virtual ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();

    public virtual ICollection<UserGenre> UserGenres { get; set; } = new List<UserGenre>();
}
