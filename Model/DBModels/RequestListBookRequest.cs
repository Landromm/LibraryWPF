using System;
using System.Collections.Generic;

namespace LibraryWPF.Model.DBModels;

public partial class RequestListBookRequest
{
    public int Number { get; set; }

    public int Id { get; set; }

    public virtual ListBookRequest IdNavigation { get; set; } = null!;

    public virtual Request NumberNavigation { get; set; } = null!;
}
