using System;
using System.Collections.Generic;

namespace Readioo.Models;

public partial class UserFriend
{
    public int UserFriendId { get; set; }

    public int UserId { get; set; }

    public int FriendId { get; set; }

    public virtual User Friend { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
