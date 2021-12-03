using ASP_Assignment.Data;
using ASP_Assignment.Repositories;
using ASP_Assignment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Assignment.Controllers
{
    public class AccountsController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        public AccountsController( ApplicationDbContext context)
        {
            
            _context = context;
        }

        public IActionResult Index(string accountTypeNum)
        {
            string type;
            
              ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);
            
            if (accountTypeNum == "0")
            {
                type = "Chequing";
               var query = esRepo.GetAll(type);
                ViewBag.Name = type;
                return View(query);
            }
            else if(accountTypeNum == "1")
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
        //public ActionResult Profile(int clientID, int accountNum)
        //{

        //    ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);
        //    ClientAccountVM caVM = esRepo.GetDetail(clientID, accountNum);
        //    return View(caVM);
        //}
        [Authorize]
        public ActionResult Profile()
        {
            string userName = User.Identity.Name;
            ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);
            ClientAccountVM caVM = esRepo.GetProfile(userName);
            return View(caVM);
        }
    }
}
