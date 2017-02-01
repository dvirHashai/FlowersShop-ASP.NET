using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreProject.Models
{
    public class Repositoy
    {
        private ApplicationDbContext _context;

        public Repositoy(ApplicationDbContext context)
        {
            _context = context;
        }

        //public IEnumerable<Supplier> getSuppliers()
        //{

        //}
    }
}
