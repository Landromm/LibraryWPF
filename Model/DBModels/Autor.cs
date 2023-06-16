using System;
using System.Collections.Generic;

namespace LibraryWPF.Model.DBModels;

public partial class Autor
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string LastName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
