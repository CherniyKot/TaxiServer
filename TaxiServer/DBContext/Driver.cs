using System;
using System.Collections.Generic;

#nullable disable

namespace TaxiServer
{
    public partial class Driver
    {
        public Driver()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Car { get; set; }
        public bool IsOnline { get; set; }

        public virtual DriverLocation DriverLocation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}

