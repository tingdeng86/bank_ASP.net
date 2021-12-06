using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Assignment.Models
{
    public class MyRegisteredUser
    {
        [Display(Name = "First Name")]
        [Required]
        [RegularExpression(@"[a-zA-Z]{1,40}",
            ErrorMessage = "This is not a valid first name.")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [Required]
        [RegularExpression(@"[a-zA-Z]{1,40}",
                    ErrorMessage = "This is not a valid last name.")]
        public string LastName { get; set; }

        [Key]
        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    ErrorMessage = "This is not a valid email.")]
        public string Email { get; internal set; }

        [Display(Name = "Account Type")]
        [Required]
        public string AccountType { get; set; }

        [Display(Name = "Balance")]
        [Required]
        [RegularExpression(@"^[0-9]\d*(\.\d{2})",
                    ErrorMessage = "Balance should be a number which must include two digits to the right of the decimal.")]
        public decimal Balance { get; set; }
    }
}
