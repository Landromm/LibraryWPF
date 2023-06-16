using System;
using System.Collections.Generic;

namespace LibraryWPF.Model.DBModels;

public partial class Rack
{
    public int StackNumber { get; set; }

    public int StorageSize { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
