using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreProject.Models
{
    public class Cupons
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public DateTime Created { get; set; }
        public bool IsUsed { get; set; }
        public string IP { get; set; }
    }
}
