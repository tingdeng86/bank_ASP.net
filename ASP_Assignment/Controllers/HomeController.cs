using ASP_Assignment.Data;
using ASP_Assignment.Models;
using ASP_Assignment.Repositories;
using ASP_Assignment.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        const string FIRSTNAME = "FirstName";
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string email = User.Identity.Name;
                ClientAccountVMRepo esRepo = new ClientAccountVMRepo(_context);
                IQueryable<ClientAccountVM> caVM = esRepo.GetLists(email);
                string firstName = caVM.First().firstName;
                HttpContext.Session.SetString(FIRSTNAME, firstName);
                HttpContext.Session.GetString(FIRSTNAME);

            }
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
