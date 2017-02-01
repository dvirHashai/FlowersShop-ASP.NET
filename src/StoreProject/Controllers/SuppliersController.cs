using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using StoreProject.Models;
using Microsoft.AspNet.Authorization;

namespace StoreProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SuppliersController : Controller
    {
        private ApplicationDbContext _context;


        public SuppliersController(ApplicationDbContext context)
        {

            _context = context;
        }

        // GET: Suppliers
        public IActionResult Index()
        {

            return View(_context.Suppliers.ToList());

        }

        // GET: Suppliers/Details/5
        public IActionResult Details(int? id)
        {


            if (id == null)
            {
                return HttpNotFound();
            }

            Supplier supplier = _context.Suppliers.Single(m => m.SupplierID == id);
            if (supplier == null)
            {
                return HttpNotFound();
            }

            return View(supplier);

        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {

            return View();


        }

        // POST: Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Supplier supplier)
        {

            if (ModelState.IsValid)
            {
                _context.Suppliers.Add(supplier);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier);



        }

        // GET: Suppliers/Edit/5
        public IActionResult Edit(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            Supplier supplier = _context.Suppliers.Single(m => m.SupplierID == id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);

        }

        // POST: Suppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Supplier supplier)
        {

            if (ModelState.IsValid)
            {
                _context.Update(supplier);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier);


        }

        // GET: Suppliers/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            Supplier supplier = _context.Suppliers.Single(m => m.SupplierID == id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            var productList =
                from s in _context.Suppliers
                join p in _context.Products
                on s.SupplierID equals p.SupplierID
                where supplier.SupplierID == p.SupplierID
                select p;
            if (productList.ToList().Count() == 0)
            {
                return View(supplier);
            }
            return RedirectToAction("Error", "Home", new { error = "You can not earse supplier with connected products" });


        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {

            Supplier supplier = _context.Suppliers.Single(m => m.SupplierID == id);
            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();
            return RedirectToAction("Index");



        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SearchBySupplier(string City)
        {
            int temp = 0;
            int.TryParse(City, out temp);
            if (temp != 0)
            {
                return RedirectToAction("Error", "Home", new { error = "You have Entered invliad string to City" });
            }
            if (City == null)
            {
                City = "";
            }
            var item = from s in _context.Suppliers
                       join p in _context.Customers
                       on s.City equals p.City
                       where p.City.Contains(City)
                       select s;
            /*Todo: Add view for this*/

            return View(item.ToList().Distinct());
            //int temp =0;
            //int.TryParse(City,out temp);
            //if(temp!=0)
            //{
            //    return RedirectToAction("Error", "Home", new { error = "You have Entered invliad string to City" });
            //}
            //var item = _context.Products.GroupBy(p => p.Category).Select(group => new
            //{   
            //    Product = group.Key,
            //    listProduct = group.ToList(),
            //    count = group.Count()

            //});
            //ViewData["items"] = item.ToList();
            //return View(_context.Products.ToList());
        }
    }
}
