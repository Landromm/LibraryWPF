using System;
using System.Collections.Generic;

namespace LibraryWPF.Model.DBModels;

public partial class LoginUser
{
    public Guid Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual User? User { get; set; }
}
