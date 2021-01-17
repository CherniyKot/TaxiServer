using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiServer.Objects
{
    public class OrderStatus
    {
        public int Status { get; set; }
        public double LongitudeFrom { get; set; }
        public double LatitudeFrom { get; set; }
        public double LongitudeTo { get; set; }
        public double LatitudeTo { get; set; }
        public double? LongitudeDriver { get; set; }
        public double? LatitudeDriver { get; set; }
        public double Cost { get; set; }
    }
}
