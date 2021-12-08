using ASP_Assignment.Data;
using ASP_Assignment.Models;
using ASP_Assignment.Repositories;
using ASP_Assignment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ASP_Assignment.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public AccountsController( ApplicationDbContext context)
        {         
            _context = context;
        }
        const string FIRSTNAME = "FirstName";
        public IActionResult Index(string message)
        {
            if (message == null)
            {
                message = "";
            }
            ViewData["Message"] = message;

            string email = User.Identity.Name;
            ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);
            IQueryable<ClientAccountVM> caVM = esRepo.GetLists(email);
            
            string firstName = caVM.First().firstName;           
            HttpContext.Session.SetString(FIRSTNAME, firstName);
            HttpContext.Session.GetString(FIRSTNAME);          
            return View(caVM);

        }
        public ActionResult Details(int clientID, int accountNum,string message)
        {
            if (message == null)
            {
                message = "";
            }
            ViewData["Message"] = message;

            ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);
            ClientAccountVM caVM = esRepo.GetDetail(clientID, accountNum);
            return View(caVM);
        }
        [HttpGet]
        public ActionResult Edit(int clientID, int accountNum)
        {
            ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);
            ClientAccountVM caVM = esRepo.GetDetail(clientID, accountNum);
            return View(caVM);
        }
        [HttpPost]
        public ActionResult Edit(ClientAccountVM caVM)
        {
            ViewData["Message"] = "";
            ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);
            if (ModelState.IsValid)
            {
                esRepo.Update(caVM);
                ViewData["Message"] = "The update has been saved.";

                return RedirectToAction("Details", "Accounts", new { caVM.clientID, caVM.accountNum, message = ViewData["Message"] });
            }
            else
            {
                ViewData["Message"] = "This entry is invalid.";
                return View(caVM);
            }           
        }

        [HttpGet]
        public ActionResult Create()
        {
            ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);            
            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind("accountType,balance")] ClientAccountVM ca)
        {
            ViewData["Message"] = "";
            ca.email = User.Identity.Name;            
            ClientAccount clientAccount;
            try {
                ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);
                clientAccount = esRepo.Add(ca);

                ViewData["Message"] = "Created successfully";
                return RedirectToAction("Details", "Accounts", new { clientAccount.clientID, clientAccount.accountNum, message = ViewData["Message"] });
            }
            catch (Exception e) {
                ViewData["Message"] =e.Message;
                return View(ca);
            }
        }
        public ActionResult Delete (int clientID,int accountNum)
        {
            ViewData["Message"] = "";
            try
            {
                ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);
                esRepo.Delete(clientID, accountNum);
                ViewData["Message"] = "Deleted successfully";
            }
            catch (Exception e)
            {
                ViewData["Message"] =e.Message;
            }
            return RedirectToAction("Index", "Accounts", new { message = ViewData["Message"] });
        }      

    }
}
