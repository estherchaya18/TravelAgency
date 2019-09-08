using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Models;

namespace TravelAgency.Controllers
{
    public class HomeController : Controller
    {
        private readonly TravelAgencyContext _context;

        public HomeController(TravelAgencyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string from, string to, DateTime departure, int passengers)
        {
            var travelAgencyContext = _context.Flights.Include(f => f.Airlines).Include(f => f.AppearanceAirport).Include(f => f.LandingAirport)
                .Where(flight => flight.AppearanceAirportId.ToString() == from && flight.LandingAirportId.ToString() == to && flight.AppppearanceDateTime.Date == departure.Date && (flight.TotalSeats - flight.ReservedSeats) >= passengers);

            //return View(await travelAgencyContext.ToListAsync());
            ViewData["from"] = from;
            ViewData["to"] = to;
            ViewData["departure"] = departure.Date.ToShortDateString();
            ViewData["passangers"] = passengers;

            ViewData["AppearanceAirportId"] = new SelectList(_context.Airports, "Id", "AirportDetailes");
            ViewData["LandingAirportId"] = new SelectList(_context.Airports, "Id", "AirportDetailes");



            return View(await travelAgencyContext.ToListAsync());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult OrderGraph()
        {
            var q = from u in _context.Flights
                    select u.Id;

            ViewBag.data = "[" + string.Join(",", q.Distinct().ToList()) + "]";
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public override ViewResult View()
        {
            if (HttpContext.Session.GetString("userId") != null)
            {
                ViewBag.userName = _context.Clients.Find(int.Parse(HttpContext.Session.GetString("userId"))).Mail;
                ViewBag.IsDirector = _context.Clients.Find(int.Parse(HttpContext.Session.GetString("userId"))).Director;
            }
            return base.View();
        }

        public override ViewResult View(object model)
        {
            if (HttpContext.Session.GetString("userId") != null)
            {
                ViewBag.userName = _context.Clients.Find(int.Parse(HttpContext.Session.GetString("userId"))).Mail;
                ViewBag.IsDirector = _context.Clients.Find(int.Parse(HttpContext.Session.GetString("userId"))).Director;
            }
            return base.View(model);
        }

    }
}
