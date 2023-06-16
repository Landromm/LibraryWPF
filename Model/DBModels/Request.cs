using System;
using System.Collections.Generic;

namespace LibraryWPF.Model.DBModels;

public partial class Request
{
    public int Number { get; set; }

    public int ListBook { get; set; }

    public DateOnly DateRegistrRequest { get; set; }

    /// <summary>
    /// True - Заявка одобрена, false - заявка на рассмотрении.
    /// </summary>
    public bool StatusRequest { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
