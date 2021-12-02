using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Assignment.ViewModels
{
    public class ClientAccountVM
    {
        [DisplayName("Account Number")] // Give nice label name for CRUD.
        public int accountNum { get; set; }
        [DisplayName("Last Name")]   // Give nice label name for CRUD.
        public string lastName { get; set; }
        [DisplayName("First Name")]  // Give nice label name for CRUD.
        public string firstName { get; set; }

        [DisplayName("AccountType")] // Give nice label name for CRUD.
        public string accountType { get; set; }
        public int clientID { get; set; }
        public decimal balance { get; set; }
        public string email { get; internal set; }


    }
}
