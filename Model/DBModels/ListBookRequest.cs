using System;
using System.Collections.Generic;

namespace LibraryWPF.Model.DBModels;

public partial class ListBookRequest
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public DateOnly? DateOfissue { get; set; }

    public DateOnly? DateReturn { get; set; }

    public int ListBooks { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual ICollection<RequestListBookRequest> RequestListBookRequests { get; set; } = new List<RequestListBookRequest>();
}
