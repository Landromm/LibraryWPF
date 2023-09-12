using System;
using System.Collections.Generic;

namespace LibraryWPF.Model.DBModels;

public partial class RequestArchive
{
    public int NumberRequest { get; set; }

    public DateTime DateRegistrRequest { get; set; }

    public virtual ICollection<BookArchive> BookArchives { get; set; } = new List<BookArchive>();
}
