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
    public class OrderPassagersController : Controller
    {
        private readonly TravelAgencyContext _context;

        public OrderPassagersController(TravelAgencyContext context)
        {
            _context = context;
        }

        // GET: OrderPassagers
        public async Task<IActionResult> Index()
        {
            var travelAgencyContext = _context.OrderPassagers.Include(o => o.Order).Include(o => o.Passanger);
            return View(await travelAgencyContext.ToListAsync());
        }

        // GET: OrderPassagers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderPassagers = await _context.OrderPassagers
                .Include(o => o.Order)
                .Include(o => o.Passanger)
                .SingleOrDefaultAsync(m => m.PassangerId == id);
            if (orderPassagers == null)
            {
                return NotFound();
            }

            return View(orderPassagers);
        }

        // GET: OrderPassagers/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id");
            ViewData["PassangerId"] = new SelectList(_context.Set<Passanger>(), "Id", "Id");
            return View();
        }

        // POST: OrderPassagers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PassangerId,OrderId")] OrderPassagers orderPassagers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderPassagers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id", orderPassagers.OrderId);
            ViewData["PassangerId"] = new SelectList(_context.Set<Passanger>(), "Id", "Id", orderPassagers.PassangerId);
            return View(orderPassagers);
        }

        // GET: OrderPassagers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderPassagers = await _context.OrderPassagers.SingleOrDefaultAsync(m => m.PassangerId == id);
            if (orderPassagers == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id", orderPassagers.OrderId);
            ViewData["PassangerId"] = new SelectList(_context.Set<Passanger>(), "Id", "Id", orderPassagers.PassangerId);
            return View(orderPassagers);
        }

        // POST: OrderPassagers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PassangerId,OrderId")] OrderPassagers orderPassagers)
        {
            if (id != orderPassagers.PassangerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderPassagers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderPassagersExists(orderPassagers.PassangerId))
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
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id", orderPassagers.OrderId);
            ViewData["PassangerId"] = new SelectList(_context.Set<Passanger>(), "Id", "Id", orderPassagers.PassangerId);
            return View(orderPassagers);
        }

        // GET: OrderPassagers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderPassagers = await _context.OrderPassagers
                .Include(o => o.Order)
                .Include(o => o.Passanger)
                .SingleOrDefaultAsync(m => m.PassangerId == id);
            if (orderPassagers == null)
            {
                return NotFound();
            }

            return View(orderPassagers);
        }

        // POST: OrderPassagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderPassagers = await _context.OrderPassagers.SingleOrDefaultAsync(m => m.PassangerId == id);
            _context.OrderPassagers.Remove(orderPassagers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderPassagersExists(int id)
        {
            return _context.OrderPassagers.Any(e => e.PassangerId == id);
        }
    }
}
