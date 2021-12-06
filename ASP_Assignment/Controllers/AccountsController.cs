using ASP_Assignment.Data;
using ASP_Assignment.Models;
using ASP_Assignment.Repositories;
using ASP_Assignment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            string email = User.Identity.Name;
            ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);
            IQueryable<ClientAccountVM> caVM = esRepo.GetLists(email);
            return View(caVM);

        }
        public ActionResult Details(int clientID, int accountNum)
        {
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
            ViewBag.ErrorMessage = "";
            ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);
            if (ModelState.IsValid)
            {
                esRepo.Update(caVM);
                return RedirectToAction("Details", "Accounts", new { caVM.clientID, caVM.accountNum });
            }
            else
            {
                ViewBag.ErrorMessage = "This entry is invalid.";
                return View(caVM);
            }           
        }

        //[Authorize]
        //public ActionResult Profile()
        //{
        //    string userName = User.Identity.Name;
        //    ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);
        //    ClientAccountVM caVM = esRepo.GetProfile(userName);
        //    return View(caVM);
        //}
        [HttpGet]
        public ActionResult Create()
        {
            ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);
            
            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind("accountType,balance")] ClientAccountVM ca)
        {

            string userName = User.Identity.Name;
            ca.email = userName;
            ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);
                esRepo.Add(ca);
                return RedirectToAction("Index", "Accounts");
        }

    }
}
