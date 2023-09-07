using System;
using System.Collections.Generic;

namespace LibraryWPF.Model.DBModels;

public partial class BookArchive
{
    public int IdBook { get; set; }

    public int NumberRequest { get; set; }

    public DateOnly DateOfissue { get; set; }

    public DateOnly DateReturn { get; set; }

    public string Title { get; set; } = null!;

    public string Serias { get; set; } = null!;

    public int YearPublich { get; set; }

    public int Pages { get; set; }

    public string AutorName { get; set; } = null!;

    public string AutorLastName { get; set; } = null!;

    public string Publisher { get; set; } = null!;

    public string ReadPlace { get; set; } = null!;

    public virtual RequestArchive NumberRequestNavigation { get; set; } = null!;
}
