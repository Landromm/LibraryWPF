using System;
using System.Collections.Generic;

namespace LibraryWPF.Model.DBModels;

public partial class User
{
    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int CardNumber { get; set; }

    public string LoginUser { get; set; } = null!;

    public virtual LoginUser LoginUserNavigation { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
