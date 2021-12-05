using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Assignment.ViewModels
{
    public class ClientAccountVM
    {
        [DisplayName("Account Number")] // Give nice label name for CRUD.
        public int accountNum { get; set; }
        [Required(ErrorMessage = "Last name required.")]
        [RegularExpression(@"[a-zA-Z]{1,40}",
                         ErrorMessage = "This is not a valid last name.")]
        [DisplayName("Last Name")]   // Give nice label name for CRUD.
        public string lastName { get; set; }
        [Required(ErrorMessage = "First name required.")]
        [RegularExpression(@"[a-zA-Z]{1,40}",
                        ErrorMessage = "This is not a valid first name.")]

        [DisplayName("First Name")]  // Give nice label name for CRUD.
        public string firstName { get; set; }

        [DisplayName("Account Type")] // Give nice label name for CRUD.
        public string accountType { get; set; }
        [DisplayName("Client ID")]
        public int clientID { get; set; }
        [Required(ErrorMessage = "Balance required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Balance can not be less than 0.")]
        [RegularExpression(@"^[1-9]\d*(\.\d{2})",
                        ErrorMessage = "Balance should be a number which must include two digits to the right of the decimal.")]
        [DisplayName("Balance")]
        public decimal balance { get; set; }
        [DisplayName("Email")]
        public string email { get; internal set; }


    }
}
