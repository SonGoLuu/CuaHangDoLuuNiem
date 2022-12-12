using System;
using System.Collections.Generic;

#nullable disable

namespace DICHOSAIGON.Models
{
    public partial class Location
    {
        public Location()
        {
            Customers = new HashSet<Customer>();
        }

        public int LocationId { get; set; }
        public string Name { get; set; }
        public int? Parent { get; set; }
        public int? Levels { get; set; }
        public int? Idhuyen { get; set; }
        public string NameWithType { get; set; }
        public int? ThuocHuyen { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
