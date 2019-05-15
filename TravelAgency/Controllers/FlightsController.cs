using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // GET: Flights
        public async Task<IActionResult> Index()
        {
            var travelAgencyContext = _context.Flights.Include(f => f.Airlines).Include(f => f.AppearanceAirport).Include(f => f.LandingAirport);
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
    }
}
