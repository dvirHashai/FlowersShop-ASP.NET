using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using StoreProject.Models;  
using System;

namespace StoreProject.Controllers
{
    public class CuponsController : Controller
    {
        private ApplicationDbContext _context;
  
        public CuponsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        public IActionResult Generate()
        {
            //-1 = exist
            // 0 =  Error not created
            // 1 = OK

            string ip = HttpContext.Connection.RemoteIpAddress.ToString();
            string key = string.Empty;
            int Status = -1; //Exist
            if (_context.Cupons.Count(x => x.IP.Equals(ip)) > 0)
                return Json(new { Status = Status, Key = key });

            
             key = Guid.NewGuid().ToString(); //Random and Unuique Key
            Cupons cupons = new Cupons();

            cupons.Key = key;
            cupons.IP = ip;
            cupons.Created = DateTime.Now;
            cupons.IsUsed = false;

            _context.Cupons.Add(cupons);

            Status = _context.SaveChanges() > 0 ? 1 : 0 ;
            if (Status==0) key = string.Empty;


            return Json(new { Status = Status,Key=key });
        }
        // GET: Cupons

    }
}
