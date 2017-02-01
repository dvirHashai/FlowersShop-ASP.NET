using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using StoreProject.Models;
using System.Collections.Generic;
using Microsoft.AspNet.Authorization;
using System.Text.RegularExpressions;
using StoreProject.ViewModels.Home;

namespace StoreProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext _context;


        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public IActionResult Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Supplier);
            return View(applicationDbContext.ToList());
        }

        // GET: Products/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Product product = _context.Products.Single(m => m.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Supplier");
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var sup in _context.Suppliers)
            {
                list.Add(new SelectListItem { Text = sup.CompanyName, Value = sup.SupplierID.ToString() });
            }
            List<SelectListItem> category = new List<SelectListItem>();
            category.Add(new SelectListItem { Text = "Gifts", Value = "Gifts" });
            category.Add(new SelectListItem { Text = "WorkShops", Value = "WorkShops" });
            category.Add(new SelectListItem { Text = "HomeMade", Value = "HomeMade" });
            category.Add(new SelectListItem { Text = "Vegtables", Value = "Vegtables" });
            category.Add(new SelectListItem { Text = "Gardening", Value = "Gardening" });
            ViewBag.Category = category;
            ViewBag.suplliers = list;
            //ViewBag.suplliers = new SelectList(_context.Suppliers, "SupplierID", "CompanyName");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                Supplier supplier = _context.Suppliers.Single(m => m.SupplierID == product.SupplierID);
                product.Supplier = supplier;
                //supplier.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Supplier", product.SupplierID);
            return View(product);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Product product = _context.Products.Single(m => m.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Supplier", product.SupplierID);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Update(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Supplier", product.SupplierID);
            return View(product);
        }

        // GET: Products/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Product product = _context.Products.Single(m => m.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Product product = _context.Products.Single(m => m.ProductID == id);
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Index")]
        public IActionResult Myajaxcall(string category)
        {
            ViewBag.filter = category;
            var applicationDbContext = _context.Products.Include(p => p.Supplier);
            return View(applicationDbContext.ToList());
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SearchCategory(Product searchString)
        {
            var products = _context.Products.Where(m => m.Category.Equals(searchString.Category));
            return View(products.ToList());
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Search(string price, string category, string supplier)
        {
            int temp = 0;
            if ((int.TryParse(supplier, out temp)) || (int.TryParse(category, out temp)))
            {
                return RedirectToAction("Error", "Home", new { error = "You have Entered invliad string to supplier /  category " });
            }
            if ((int.TryParse(price, out temp)))
            {
                if (temp < 0)
                {
                    return RedirectToAction("Error", "Home", new { error = "You have Entered invliad number to price" });
                }
            }
            //}else
            //{
            //    return RedirectToAction("Error", "Home", new { error = "You have Entered invliad number to price" });
            //}

            int _price;
            if ((price == null) || !(System.Int32.TryParse(price,out _price)))
            {
                if (category == null)
                {
                    category = @"[A-Z]";
                }
                if (supplier == null)
                {
                    supplier = @"[A-Z]";
                }
                var item1 = from s in _context.Suppliers
                           join p in _context.Products
                           on s.SupplierID equals p.SupplierID
                           where s.CompanyName.Contains(supplier) && p.Category.Contains(category)
                           select p;
                return View(item1.ToList());
            }
            else
            {
                _price = System.Int32.Parse(price);
            }
            if (category == null)
            {
                category = @"[A-Z]";
            }
            if (supplier == null)
            {
                supplier = @"[A-Z]";
            }

            var item = from s in _context.Suppliers
                       join p in _context.Products
                       on s.SupplierID equals p.SupplierID
                       where p.UnitPrice <= _price && s.CompanyName.Contains(supplier) && p.Category.Contains(category)
                       select p;
            return View(item.ToList());
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SearchByName(string name)
        {
            int temp=0;
            if (int.TryParse(name, out temp)){
                return RedirectToAction("Error", "Home", new { error = "You have Entered invliad name" });
            }
            var products = _context.Products.Where(m => m.ProductName.Contains(name));
            return View(products.ToList());

        }

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult SearchBySupplier(string City)
        //{
        //    //if (City == null)
        //    //{
        //    //    City = "";
        //    //}
        //    //var item = from s in _context.Suppliers
        //    //           join p in _context.Customers
        //    //           on s.City equals p.City
        //    //           where p.City.Contains(City)
        //    //           select s;
        //    ///*Todo: Add view for this*/
        //    //return View(item.ToList());


        //}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult InventoryCheck()
        {
            var item = _context.Products.GroupBy(p => p.Category).Select(g => new InventoryCheck
            {
                category = g.Key,
                count = g.Count()

            });
            ViewData["InventoryList"] = item.ToList();
            return View();
        }
    }
}
