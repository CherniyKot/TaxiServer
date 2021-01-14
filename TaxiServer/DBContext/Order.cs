using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

#nullable disable

namespace TaxiServer
{
    public partial class Order
    {
        public Order()
        {
            Ratings = new HashSet<Rating>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public Point LocationFrom { get; set; }
        public Point LocationTo { get; set; }
        public int? DriverId { get; set; }
        public int? Status { get; set; }

        public virtual Driver Driver { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
