using System;
using System.Collections.Generic;

namespace LibraryWPF.Model.DBModels;

public partial class Role
{
    public int Id { get; set; }

    public string RoleUsers { get; set; } = null!;

    public virtual ICollection<LoginUser> LoginUsers { get; set; } = new List<LoginUser>();
}
