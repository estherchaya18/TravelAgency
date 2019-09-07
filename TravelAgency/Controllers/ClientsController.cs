using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Models;
using System.Web;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace TravelAgency.Controllers
{
    public class ClientsController : Controller
    {
        private readonly TravelAgencyContext _context;

        public ClientsController(TravelAgencyContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clients.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clients = await _context.Clients
                .SingleOrDefaultAsync(m => m.Id == id);
            if (clients == null)
            {
                return NotFound();
            }

            return View(clients);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Mail,Password,Director")] Clients clients)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clients);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clients);
        }

        public IActionResult Register()
        {
            ViewBag.Fail = false;
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Mail,Password,Director")] Clients clients)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clients);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            ViewBag.Fail = true;
            return View(clients);
        }

        public IActionResult Login()
        {
            ViewBag.Fail = false;
            return View();
        }

        public async Task<IActionResult> Logout()
        {
                //HttpContext.Session.SetString("userId", (result.First().Id).ToString());
            HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Id,Mail,Password")] Clients user)
        {
            var result = from u in _context.Clients
                         where u.Mail == user.Mail && u.Password == user.Password
                         select u;

            if (result.ToList().Count > 0)
            {
                HttpContext.Session.SetString("userId", (result.First().Id).ToString());
                if (HttpContext.Session.GetString("userId") != null)
                {
                    ViewBag.userName = _context.Clients.Find(int.Parse(HttpContext.Session.GetString("userId"))).Mail;
                    ViewBag.IsDirector = _context.Clients.Find(int.Parse(HttpContext.Session.GetString("userId"))).Director;
                }
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Fail = true;



            return View(user);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clients = await _context.Clients.SingleOrDefaultAsync(m => m.Id == id);
            if (clients == null)
            {
                return NotFound();
            }
            return View(clients);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Mail,Password,Director")] Clients clients)
        {
            if (id != clients.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clients);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientsExists(clients.Id))
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
            return View(clients);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clients = await _context.Clients
                .SingleOrDefaultAsync(m => m.Id == id);
            if (clients == null)
            {
                return NotFound();
            }

            return View(clients);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clients = await _context.Clients.SingleOrDefaultAsync(m => m.Id == id);
            _context.Clients.Remove(clients);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientsExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
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
