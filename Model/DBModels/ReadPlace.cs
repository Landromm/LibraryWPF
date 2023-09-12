using System;
using System.Collections.Generic;

namespace LibraryWPF.Model.DBModels;

public partial class ReadPlace
{
    public int Id { get; set; }

    public string ReadPlace1 { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
