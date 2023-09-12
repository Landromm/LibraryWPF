using System;
using System.Collections.Generic;

namespace LibraryWPF.Model.DBModels;

public partial class Request
{
    public int Number { get; set; }

    public int ListBook { get; set; }

    public DateTime DateRegistrRequest { get; set; }

    /// <summary>
    /// True - Заявка одобрена, false - заявка на рассмотрении.
    /// </summary>
    public bool StatusRequest { get; set; }

    public int? UserCardNumber { get; set; }

    public virtual ICollection<RequestListBookRequest> RequestListBookRequests { get; set; } = new List<RequestListBookRequest>();

    public virtual User? UserCardNumberNavigation { get; set; }
}
