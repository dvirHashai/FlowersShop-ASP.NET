using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreProject.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "This is a required filed")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Please enter valid string")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter valid name")]

        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "This is a required filed")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Please enter valid string")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter valid name")]

        public string LastName { get; set; }

        [Required(ErrorMessage = "This is a required filed")]
        public string Address { get; set; }

        [Display(Name = "Zip Code")]
        [Range(1000000, 9999999, ErrorMessage = "Please enter Valid Integer Number")]
        [Required(ErrorMessage = "This is a required item")]
        public int ZipCode { get; set; }

        [Required(ErrorMessage = "This is a required filed")]
        public string City { get; set; }

        [Required(ErrorMessage = "This is a required filed")]
        [EmailAddress(ErrorMessage = "Please enter a valid Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This is a required filed")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string Phone { get; set; }

        [Display(Name = "BirthDay")]
        [Required(ErrorMessage = "This is required filed")]
        [DataType(DataType.Date, ErrorMessage = "Please enter correct Date time")]
        public DateTime Bday { get; set; }        
    }
}
