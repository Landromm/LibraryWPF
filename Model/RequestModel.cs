using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWPF.Model
{
    public class RequestModel
    {
        public int NumberRequest { get; set; }
        public DateTime DateRegistred { get; set; }
        public int? UserCardNumber { get; set; }
        public string? UserName { get; set; }
        public string? UserLastName { get; set; }

        public List<MoreRequestModel>? moreRequestModels { get; set; }
    }
}
