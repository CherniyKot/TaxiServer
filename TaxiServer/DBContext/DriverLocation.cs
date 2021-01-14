using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

#nullable disable

namespace TaxiServer
{
    public partial class DriverLocation
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public Point Location { get; set; }
        public DateTime? Time { get; set; }

        public virtual Driver Driver { get; set; }
    }
}
