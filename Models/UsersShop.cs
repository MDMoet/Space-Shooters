using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class UsersShop
{
    public int UserId { get; set; }

    public int UsersShopitemId { get; set; }

    public int UserItemId { get; set; }

    public int Worth { get; set; }

    public int Amount { get; set; }

    public virtual User User { get; set; } = null!;
}
