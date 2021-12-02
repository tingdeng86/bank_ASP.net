using ASP_Assignment.Data;
using ASP_Assignment.Repositories;
using ASP_Assignment.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Assignment.Controllers
{
    public class AccountsController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        public AccountsController( ApplicationDbContext context)
        {
            
            _context = context;
        }

        public IActionResult Index(string accountTypeNUm)
        {
            string type;
            
            ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);
            
            if (accountTypeNUm == "0")
            {
                type = "Chequing";
               var query = esRepo.GetAll(type);
                ViewBag.Name = type;
                return View(query);


            }
            else if(accountTypeNUm == "1")
            {
                type = "Savings";
                var query = esRepo.GetAll(type);
                ViewBag.Name = type;
                return View(query);
            }
            else
            {
                type = "All";
               var  query = esRepo.GetAll();
                ViewBag.Name = type;
                return View(query);
            }
  
        }
    }
}
