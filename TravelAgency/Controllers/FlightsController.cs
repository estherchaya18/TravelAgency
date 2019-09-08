using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Models;

namespace TravelAgency.Controllers
{
    public class FlightsController : Controller
    {
        private readonly TravelAgencyContext _context;

        public FlightsController(TravelAgencyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var travelAgencyContext = _context.Flights.Include(f => f.Airlines);
            return View(await travelAgencyContext.ToListAsync());
        }

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flights = await _context.Flights
                .Include(f => f.Airlines)
                .Include(f => f.AppearanceAirport)
                .Include(f => f.LandingAirport)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (flights == null)
            {
                return NotFound();
            }

            return View(flights);
        }

        // GET: Flights/Create
        public IActionResult Create()
        {
            ViewData["AirlinesId"] = new SelectList(_context.Airlines, "Id", "Name");
            ViewData["AppearanceAirportId"] = new SelectList(_context.Airports, "Id", "AirportDetailes");
            ViewData["LandingAirportId"] = new SelectList(_context.Airports, "Id", "AirportDetailes");
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppearanceAirportId,LandingAirportId,AppppearanceDateTime,LandingDateTime,AppearanceTerminal,LandingTerminal,Price,TotalSeats,ReservedSeats,AirlinesId")] Flights flights)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flights);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AirlinesId"] = new SelectList(_context.Airlines, "Id", "Id", flights.AirlinesId);
            ViewData["AppearanceAirportId"] = new SelectList(_context.Airports, "Id", "Id", flights.AppearanceAirportId);
            ViewData["LandingAirportId"] = new SelectList(_context.Airports, "Id", "Id", flights.LandingAirportId);
            return View(flights);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flights = await _context.Flights.SingleOrDefaultAsync(m => m.Id == id);
            if (flights == null)
            {
                return NotFound();
            }
            ViewData["AirlinesId"] = new SelectList(_context.Airlines, "Id", "Id", flights.AirlinesId);
            ViewData["AppearanceAirportId"] = new SelectList(_context.Airports, "Id", "Id", flights.AppearanceAirportId);
            ViewData["LandingAirportId"] = new SelectList(_context.Airports, "Id", "Id", flights.LandingAirportId);
            return View(flights);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppearanceAirportId,LandingAirportId,AppppearanceDateTime,LandingDateTime,AppearanceTerminal,LandingTerminal,Price,TotalSeats,ReservedSeats,AirlinesId")] Flights flights)
        {
            if (id != flights.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flights);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightsExists(flights.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AirlinesId"] = new SelectList(_context.Airlines, "Id", "Id", flights.AirlinesId);
            ViewData["AppearanceAirportId"] = new SelectList(_context.Airports, "Id", "Id", flights.AppearanceAirportId);
            ViewData["LandingAirportId"] = new SelectList(_context.Airports, "Id", "Id", flights.LandingAirportId);
            return View(flights);
        }

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flights = await _context.Flights
                .Include(f => f.Airlines)
                .Include(f => f.AppearanceAirport)
                .Include(f => f.LandingAirport)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (flights == null)
            {
                return NotFound();
            }

            return View(flights);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flights = await _context.Flights.SingleOrDefaultAsync(m => m.Id == id);
            _context.Flights.Remove(flights);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightsExists(int id)
        {
            return _context.Flights.Any(e => e.Id == id);
        }

        public async Task<IActionResult> SortList(string sortOrder, string from, string to, string departure, int passengers)
        {
            var flightsSearch = _context.Flights.Include(f => f.Airlines).Include(f => f.AppearanceAirport).Include(f => f.LandingAirport)
                      .Where(flight => flight.AppearanceAirportId.ToString() == from && flight.LandingAirportId.ToString() == to 
                       && (flight.TotalSeats - flight.ReservedSeats) >= passengers)
                      .OrderBy(f => f.Id);

            switch (sortOrder)
            {
                case "Price":
                    flightsSearch = flightsSearch.Where(f=>f.AppppearanceDateTime.Date.ToShortDateString()==departure).OrderBy(s => s.Price);
                    break;
                case "Fastest":
                    flightsSearch = flightsSearch.Where(f => f.AppppearanceDateTime.Date.ToShortDateString() == departure).OrderBy(s => (s.LandingDateTime - s.AppppearanceDateTime).TotalSeconds);
                    break;
                default:
                    //flights = flights;
                    break;
            }
            if (flightsSearch == null)
                return View(new List<Flights>());
            return PartialView("_SortList", await flightsSearch.ToListAsync());
        }


        public async Task<IActionResult> FilterTimeList(string from, string to, string departure, int passengers, string startTime, string endTime)
        {
            DateTime StartDeparture = Convert.ToDateTime(departure);
            if (startTime.Substring(5, 2) == "AM")
                StartDeparture = StartDeparture.AddHours(int.Parse(startTime.Substring(0, 2))==12?0: int.Parse(startTime.Substring(0, 2)));
            else
                StartDeparture = StartDeparture.AddHours(int.Parse(startTime.Substring(0, 2))+12);
            StartDeparture = StartDeparture.AddMinutes(int.Parse(startTime.Substring(3, 2)));

            DateTime endDeparture = Convert.ToDateTime(departure);
            if (endTime.Substring(5, 2) == "AM")
                endDeparture = endDeparture.AddHours(int.Parse(endTime.Substring(0, 2)));
            else
                endDeparture = endDeparture.AddHours(int.Parse(endTime.Substring(0, 2))<12?(int.Parse(endTime.Substring(0, 2)) + 12):(int.Parse(endTime.Substring(0, 2))));
            endDeparture = endDeparture.AddMinutes(int.Parse(endTime.Substring(3, 2)));

            var flightsSearch = _context.Flights.Include(f => f.Airlines).Include(f => f.AppearanceAirport).Include(f => f.LandingAirport)
                      .Where(flight => flight.AppearanceAirportId.ToString() == from && flight.LandingAirportId.ToString() == to
                       && (flight.TotalSeats - flight.ReservedSeats) >= passengers)
                      .OrderBy(f => f.Id);

            flightsSearch = flightsSearch.Where(f => f.AppppearanceDateTime.Date.ToShortDateString() == departure).OrderBy(f => f.Id);
            flightsSearch = flightsSearch.Where(f =>  f.AppppearanceDateTime > StartDeparture).OrderBy(f => f.Id);
            flightsSearch = flightsSearch.Where(f =>  f.AppppearanceDateTime < endDeparture).OrderBy(f => f.Id);
            //switch (sortOrder)
            //{
            //    case "Price":
            //        flightsSearch = flightsSearch.Where(f => f.AppppearanceDateTime.Date.ToShortDateString() == departure).OrderBy(s => s.Price);
            //        break;
            //    case "Fastest":
            //        flightsSearch = flightsSearch.Where(f => f.AppppearanceDateTime.Date.ToShortDateString() == departure).OrderBy(s => (s.LandingDateTime - s.AppppearanceDateTime).TotalSeconds);
            //        break;
            //    default:
            //        //flights = flights;
            //        break;
            //}
            if (flightsSearch == null)
                return View(new List<Flights>());
            return PartialView("_FilterTimeList", await flightsSearch.ToListAsync());
        }

        public ActionResult ConfirmOrder(int Id, int passangers)
        {
            var flights = _context.Flights.Include(f => f.Airlines)
                .Include(f => f.AppearanceAirport).Include(f => f.LandingAirport).Where(f => f.Id == Id).First();
            ViewData["passengers"] = passangers;
            return PartialView("_ConfirmOrder", flights);
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

        public override RedirectToActionResult RedirectToAction(string actionName)
        {
            if (HttpContext.Session.GetString("userId") != null)
            {
                ViewBag.userName = _context.Clients.Find(int.Parse(HttpContext.Session.GetString("userId"))).Mail;
                ViewBag.IsDirector = _context.Clients.Find(int.Parse(HttpContext.Session.GetString("userId"))).Director;
            }
            return base.RedirectToAction(actionName);
        }
    }
}
