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
    public class OrdersController : Controller
    {
        private readonly TravelAgencyContext _context;

        public OrdersController(TravelAgencyContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Order.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FlightId,ClientId,DateOrder")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FlightId,ClientId,DateOrder")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.SingleOrDefaultAsync(m => m.Id == id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }

        public async Task<IActionResult> OrderHistory()
        {
            var currentUserID = _context.Clients.Find(int.Parse(HttpContext.Session.GetString("userId"))).Id;
            var UserOrderHistory = _context.Order.Where(o => o.ClientId == currentUserID);
            return View(await UserOrderHistory.ToListAsync());
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
