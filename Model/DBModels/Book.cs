using System;
using System.Collections.Generic;

namespace LibraryWPF.Model.DBModels;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Serias { get; set; }

    public int YearPublich { get; set; }

    public int Pages { get; set; }

    public int AutorId { get; set; }

    public int StackNumber { get; set; }

    public int ReadPlace { get; set; }

    public string Publisher { get; set; } = null!;

    /// <summary>
    /// True - если книга в налии, false - если книга на руках
    /// </summary>
    public bool CheckAvailability { get; set; }

    public virtual Autor Autor { get; set; } = null!;

    public virtual ICollection<ListBookRequest> ListBookRequests { get; set; } = new List<ListBookRequest>();

    public virtual ReadPlace ReadPlaceNavigation { get; set; } = null!;

    public virtual Rack StackNumberNavigation { get; set; } = null!;
}
