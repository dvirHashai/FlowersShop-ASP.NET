    using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity.Metadata.Internal;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace StoreProject.Models
{
    public class Supplier
    {
        [Required]
        public int SupplierID { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "This is a required filed")]
        public string CompanyName { get; set; }

        [Display(Name = "Contact Name")]
        [Required(ErrorMessage = "This is a required filed")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter valid name")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "This is a required filed")]
        public string Address { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter valid name")]
        [Required(ErrorMessage = "This is a required filed")]
        public string City { get; set; }

        [MinLength(9,ErrorMessage= "Please enter minimum 9 digits")]
        [Required(ErrorMessage = "This is a required filed")]
        [Phone(ErrorMessage = "Please enter valid phone number")]
        public string Phone { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
