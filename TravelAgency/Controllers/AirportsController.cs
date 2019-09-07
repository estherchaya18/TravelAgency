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
    public class AirportsController : Controller
    {
        private readonly TravelAgencyContext _context;

        public AirportsController(TravelAgencyContext context)
        {
            _context = context;
        }

        // GET: Airports
        public async Task<IActionResult> Index()
        {
            return View(await _context.Airports.ToListAsync());
        }

        // GET: Airports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airports = await _context.Airports
                .SingleOrDefaultAsync(m => m.Id == id);
            if (airports == null)
            {
                return NotFound();
            }

            return View(airports);
        }

        // GET: Airports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Airports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AirportDetailes")] Airports airports)
        {
            if (ModelState.IsValid)
            {
                _context.Add(airports);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(airports);
        }

        // GET: Airports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airports = await _context.Airports.SingleOrDefaultAsync(m => m.Id == id);
            if (airports == null)
            {
                return NotFound();
            }
            return View(airports);
        }

        // POST: Airports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AirportDetailes")] Airports airports)
        {
            if (id != airports.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(airports);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirportsExists(airports.Id))
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
            return View(airports);
        }

        // GET: Airports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airports = await _context.Airports
                .SingleOrDefaultAsync(m => m.Id == id);
            if (airports == null)
            {
                return NotFound();
            }

            return View(airports);
        }

        // POST: Airports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airports = await _context.Airports.SingleOrDefaultAsync(m => m.Id == id);
            _context.Airports.Remove(airports);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AirportsExists(int id)
        {
            return _context.Airports.Any(e => e.Id == id);
        }

        public override ViewResult View()
        {
            if(HttpContext.Session.GetString("userId") != null)
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

