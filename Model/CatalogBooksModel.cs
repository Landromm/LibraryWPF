using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWPF.Model
{
    public class CatalogBooksModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? NameAutor { get; set; }
        public string? LastNameAutor { get; set; }
        public string? Serias { get; set; }
        public string? Publisher { get; set; }
        public int YearPublich { get; set; }
        public int Pages { get; set; }
        public int StackNumber { get; set; }
        public string? ReadPlace { get; set; }
        public bool CheckAvailability { get; set; }
    }
}
