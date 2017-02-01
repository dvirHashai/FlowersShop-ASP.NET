using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StoreProject.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "This is a required filed")]
        public string ProductName { get; set; }

        [Display(Name = "Unit Price")]
        [Range(0, 999999, ErrorMessage = "Please enter valid price")]
        [Required(ErrorMessage = "This is a required filed")]
        public decimal UnitPrice { get; set; }

        public string Picture { get; set; }

        public string Description { get; set; }

        [Required]
        public string Category { get; set; }


        /*Forgein Key*/
        public int SupplierID { get; set; }

        [ForeignKey("SupplierID")]
        public Supplier Supplier { get; set; }

    }
}
