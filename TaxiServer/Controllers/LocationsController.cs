using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<DriverLocation_> DriverLocations()
        {
            IEnumerable<DriverLocation_> result = db.DriverLocations.Where(d => d.Driver.IsOnline && (d.Time ?? DateTime.Now) >= DateTime.Now.AddMinutes(-5)).Select(d => new DriverLocation_()
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
        public DriverLocation_ DriverLocation(int userId)
        {
            var t = db.Orders.Where(d => d.Status == 1 && d.UserId == userId).First();
            if (t.DriverId == null) return null;

            return new DriverLocation_()
            {
                DriverID = t.Driver.Id,
                DriverName = t.Driver.Name,
                DriverCar = t.Driver.Car,
                Latitude = t.Driver.DriverLocation.Location.X,
                Longitude = t.Driver.DriverLocation.Location.Y
            };
        }

        [HttpGet]
        public OrderLocation_ OrderLocation(int driverId)
        {
            var t = db.Orders.Where(d => d.Status == 1 && d.DriverId == driverId);
            if (t.Count() == 0) return null;
            var t1 = t.First();

            return new OrderLocation_()
            {
                OrderID = t1.Id,
                UserName = t1.User.Name,
                LatitudeFrom = t1.LocationFrom.X,
                LongitudeTo = t1.LocationFrom.Y,
                LatitudeTo = t1.LocationTo.X,
                LongitudeFrom = t1.LocationTo.Y
            };
        }
    }
}
