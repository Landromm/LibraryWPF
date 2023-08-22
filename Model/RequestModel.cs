﻿using System;
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
        public DateOnly? DateOfissue { get; set; }
        public DateOnly? DateReturn { get; set; }
        public string? Title { get; set; }
        public string? Serias { get; set; }
        public int YearPublish { get; set; }
        public string? AutorName { get; set; }  
        public string? AutorLastName { get; set; }
        public string? ReadPlaces { get; set; }
        public int RackNumber { get; set; }
    }
}
