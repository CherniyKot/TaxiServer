using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiServer.Objects
{
    public class OrderLocation_
    {
        public int OrderID { get; set; }
        public string UserName { get; set; }
        public double LongitudeFrom { get; set; }
        public double LatitudeFrom { get; set; }
        public double LongitudeTo { get; set; }
        public double LatitudeTo { get; set; }
    }
}
