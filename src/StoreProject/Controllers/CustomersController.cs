using System.Linq;
using Microsoft.AspNet.Mvc;
using StoreProject.Models;
using Microsoft.AspNet.Authorization;

namespace StoreProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public IActionResult Index()
        {
            return View(_context.Customers.ToList());
        }

        // GET: Customers/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Customer customer = _context.Customers.Single(m => m.CustomerID == id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Customer customer = _context.Customers.Single(m => m.CustomerID == id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Customer = customer;
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Update(customer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Customer customer = _context.Customers.Single(m => m.CustomerID == id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Customer customer = _context.Customers.Single(m => m.CustomerID == id);
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Search(string firstName, string city, string mounth)
        {
            int temp = 0;
            int _mounth = 1;

            if ((System.Int32.TryParse(firstName, out temp)) || (System.Int32.TryParse(city, out temp)) || (System.Int32.TryParse(mounth, out _mounth)))
            {
                return RedirectToAction("Error", "Home", new { error = "You have Entered invliad string to City / First name " });
            }

            if (mounth == null)
            {



                if (firstName == null)
                {
                    firstName = @"[A-Z]";
                }
                if (city == null)
                {
                    city = @"[A-Z]";
                }
                var item1 = from s in _context.Customers
                            where s.FirstName.Contains(firstName) && s.City.Contains(city)
                            select s;
                return View(item1.ToList());
            }

            if ((_mounth <= 0) || (_mounth > 13))
            {


                return RedirectToAction("Error", "Home", new { error = "You have Entered invliad number " });
            }

            else
            {
                _mounth = System.Int32.Parse(mounth);
            }
            if (firstName == null)
            {
                firstName = @"[A-Z]";
            }
            if (city == null)
            {
                city = @"[A-Z]";
            }

            var item = from s in _context.Customers
                       where s.FirstName.Contains(firstName) && s.City.Contains(city) && s.Bday.Month.Equals(_mounth)
                       select s;
            return View(item.ToList());
        }
    }
}
