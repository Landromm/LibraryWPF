using System;
using System.Collections.Generic;

namespace LibraryWPF.Model.DBModels;

public partial class TempListBook
{
    public int Id { get; set; }

    public int IdBook { get; set; }

    public int CardNumberUser { get; set; }

    public virtual User CardNumberUserNavigation { get; set; } = null!;

    public virtual ListBookRequest? ListBookRequest { get; set; }
}
