using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxiServer.Objects;

namespace TaxiServer.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        TaxiContext db;
        public ApiController()
        {
            db = new TaxiContext();
        }
        ~ApiController()
        {
            db.Dispose();
        }

        [HttpGet]
        public IEnumerable<DriverLocation_> FreeDriversLocations()
        {
            IEnumerable<DriverLocation_> result = db.DriverLocations.Where(
                d => d.Driver.IsOnline &&
                (d.Time ?? DateTime.Now) >= DateTime.Now.AddMinutes(-5) &&
                d.Driver.Orders.Where(e => e.Status == 1 || e.Status == 2).Count() == 0).Select(
                d => new DriverLocation_()
                {
                    DriverID = d.Driver.Id,
                    DriverName = d.Driver.Name,
                    DriverCar = d.Driver.Car,
                    Latitude = d.Location.X,
                    Longitude = d.Location.Y
                }).ToArray();
            return result;
        }

        [HttpGet]
        public IEnumerable<OrderLocation_> OrderLocations()
        {
            IEnumerable<OrderLocation_> result = db.Orders.Where(d => d.Status == 1).Select(d => new OrderLocation_()
            {
                OrderID = d.Id,
                UserName = d.User.Name,
                LatitudeFrom = d.LocationFrom.X,
                LongitudeTo = d.LocationFrom.Y,
                LatitudeTo = d.LocationTo.X,
                LongitudeFrom = d.LocationTo.Y
            }).ToArray();
            return result;
        }

        [HttpGet]
        public OrderStatus UserOrder(int userId)
        {
            var t = db.Orders.Where(d => d.Status.HasValue && d.Status < 4 && d.UserId == userId).ToArray().FirstOrDefault();
            if (t == null) return new OrderStatus();
            var r = new OrderStatus()
            {
                Status = t.Status.Value,
                LatitudeFrom = t.LocationFrom.X,
                LongitudeTo = t.LocationFrom.Y,
                LatitudeTo = t.LocationTo.X,
                LongitudeFrom = t.LocationTo.Y,
                Cost = t.Cost
            };

            var t1 = db.Drivers.Where(d => d.Id == t.DriverId).Select(d=>d.DriverLocation).ToArray().FirstOrDefault();
            if (t1 != null)
            {
                r.LatitudeDriver = t1.Location.X;
                r.LongitudeDriver = t1.Location.Y;
            }
            return r;
        }

        [HttpGet]
        public OrderStatus DriverOrder(int driverId)
        {
            var t = db.Orders.Where(d => d.Status.HasValue && d.Status < 4 && d.DriverId == driverId).ToArray().FirstOrDefault();
            if (t == null) return new OrderStatus();
            var r = new OrderStatus()
            {
                Status = t.Status.Value,
                LatitudeFrom = t.LocationFrom.X,
                LongitudeTo = t.LocationFrom.Y,
                LatitudeTo = t.LocationTo.X,
                LongitudeFrom = t.LocationTo.Y,
                Cost = t.Cost
            };

            var t1 = db.Drivers.Where(d => d.Id == t.DriverId).Select(d => d.DriverLocation).ToArray().FirstOrDefault();
            if (t1 != null)
            {
                r.LatitudeDriver = t1.Location.X;
                r.LongitudeDriver = t1.Location.Y;
            }
            return r;
        }

        [HttpPost]
        public int CreateOrder([FromForm] int userId, [FromForm] double longitudeFrom, [FromForm] double latitudeFrom, [FromForm] double longitudeTo, [FromForm] double latitudeTo)
        {
            if (db.Orders.Any(e => e.UserId==userId && e.Status != 4)) return -1;
            var conn = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=Taxi;Trusted_Connection=True;");
            conn.Open();
            SqlCommand myCommand = new SqlCommand($"insert into dbo.Orders values({userId}, geography::STGeomFromText('Point({longitudeFrom.ToString("F9").Replace(',', '.')} {latitudeFrom.ToString("F9").Replace(',', '.')})', 4326), geography::STGeomFromText('Point({longitudeTo.ToString("F9").Replace(',', '.') } {latitudeTo.ToString("F9").Replace(',', '.')})', 4326), null, 1, 0)", conn);
            var r = myCommand.ExecuteNonQuery();
            conn.Dispose();
            return r;
        }

        [HttpPost]
        public int Connect([FromForm] int orderId, [FromForm] int driverId, [FromForm] double cost)
        {
            var conn = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=Taxi;Trusted_Connection=True;");
            conn.Open();
            SqlCommand myCommand = new SqlCommand($"update dbo.Orders set DriverID={driverId}, Cost={cost.ToString().Replace(',', '.')}, Status=2 where ID={orderId}", conn);
            var r = myCommand.ExecuteNonQuery();
            conn.Dispose();
            return r;
        }

        [HttpPost]
        public int UpdateLocation([FromForm] int driverId, [FromForm] double longitude, [FromForm] double latitude)
        {
            var conn = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=Taxi;Trusted_Connection=True;");
            conn.Open();
            SqlCommand myCommand = new SqlCommand($"update DriverLocations set Time='{DateTime.Now.ToString("s")}', Location=geography::STGeomFromText('Point({longitude.ToString("F9").Replace(',', '.')} {latitude.ToString("F9").Replace(',', '.')})', 4326) where DriverID={driverId}", conn);
            var r = myCommand.ExecuteNonQuery();
            conn.Dispose();
            return r;
        }

        [HttpPost]
        public int SetOrderStatus([FromForm] int driverId, [FromForm] int orderStatus)
        {
            var conn = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=Taxi;Trusted_Connection=True;");
            conn.Open();
            SqlCommand myCommand = new SqlCommand($"update Orders set Status={orderStatus} where Status<4 and DriverID={driverId}", conn);
            var r = myCommand.ExecuteNonQuery();
            conn.Dispose();
            return r;
        }
        [HttpPost]
        public int CancelOrder([FromForm] int userId)
        {
            var conn = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=Taxi;Trusted_Connection=True;");
            conn.Open();
            SqlCommand myCommand = new SqlCommand($"update Orders set Status=5 where Status<4 and UserId={userId}", conn);
            var r = myCommand.ExecuteNonQuery();
            conn.Dispose();
            return r;
        }
    }
}
