using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiServer.Objects
{
    public class DriverLocation_
    {
        public int DriverID { get; set; }
        public string DriverName { get; set; }
        public string DriverCar { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
    }
}
