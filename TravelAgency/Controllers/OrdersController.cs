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

        public async Task<IActionResult> NewOrder(string flightId, string passangersNames, string passangersPassports, string passangersBirthdates)
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                RedirectToAction("Login", "Clients");
            }

            List<string> names = passangersNames.Split(',').ToList();
            List<string> passports = passangersPassports.Split(',').ToList();
            List<string> birthdates = passangersBirthdates.Split(',').ToList();


            Order newOrder = new Order();
            newOrder.ClientId = int.Parse(HttpContext.Session.GetString("userId"));
            newOrder.DateOrder = DateTime.Now;
            newOrder.FlightId = int.Parse(flightId);

            _context.Add(newOrder);
            _context.SaveChanges();

            int lastOrderId = newOrder.Id;


            List<Passanger> passangers = new List<Passanger>();

            for (int i = 0; i < names.Count; i++)
            {
                //insert passanger
                Passanger p = new Passanger();
                p.Name = names[i];
                p.PassportId = passports[i];
                p.BirthDate = DateTime.Parse(birthdates[i]);
                _context.Add(p);
                _context.SaveChanges();
                int lastPassangerId = p.Id;

                //insert order passanges
                OrderPassagers op = new OrderPassagers();
                op.PassangerId = lastPassangerId;
                op.OrderId = lastOrderId;
                _context.Add(op);
                await _context.SaveChangesAsync();
            }

            return PartialView("OrderComplited");
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
            var UserOrderHistory = _context.Order.Where(o => o.ClientId == currentUserID).OrderByDescending(f=>f.DateOrder);
            List<orderDetails> orderDetails = new List<orderDetails>();
            UserOrderHistory.ToList().ForEach(order => {
                var orderDetail = new orderDetails();
                orderDetail.date = order.DateOrder;

                var flight = _context.Flights.Include(f=>f.AppearanceAirport).Include(f=>f.LandingAirport)
                .Include(f=>f.Airlines).Where(f=>f.Id == order.FlightId).First();
                orderDetail.flight = flight;

                orderDetail.passangers = new List<Passanger>();
                var pass = from passanger in _context.Passanger
                           join orderPass in _context.OrderPassagers on passanger.Id equals orderPass.PassangerId
                           where orderPass.OrderId == order.Id
                           select passanger;

                orderDetail.passangers = pass.ToList();

                orderDetail.id = order.Id;
                orderDetails.Add(orderDetail);
            });
            ViewBag.orders = orderDetails;
            return View(await UserOrderHistory.ToListAsync());
        }

        public IActionResult YearlyOrdersGraph()
        {
            var q = (from u in _context.Order
                    .Where(o=>o.DateOrder.Year == 2019 && o.DateOrder.Month >=6 && o.DateOrder.Month<=9)
                    .GroupBy(o=>o.DateOrder.Month)
                    .Select(g=>g.Count()) select u).ToList(); 

            ViewBag.data = "[" + string.Join(",", q.Distinct().ToList()) + "]";
            return View();
        }

        public IActionResult ClientOrdersGraph()
        {
            var q = (from u in _context.Order
                    .Where(o => o.DateOrder.Year <= 2019 && o.DateOrder.Year >= 16 && o.ClientId.ToString() == HttpContext.Session.GetString("userId"))
                    .GroupBy(o => o.DateOrder.Year)
                    .Select(g => g.Count())
                     select u).ToList();

            ViewBag.data = "[" + string.Join(",", q.Distinct().ToList()) + "]";
            return View();
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

    public class orderDetails
    {
        public DateTime date;
        public Flights flight;
        public List<Passanger> passangers;
        public int id;
    }
}
