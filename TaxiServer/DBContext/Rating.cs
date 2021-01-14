using System;
using System.Collections.Generic;

#nullable disable

namespace TaxiServer
{
    public partial class Rating
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Rating1 { get; set; }
        public string Comment { get; set; }

        public virtual Order Order { get; set; }
    }
}
