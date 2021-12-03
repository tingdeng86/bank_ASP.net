using ASP_Assignment.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using static ASP_Assignment.Data.ApplicationDbContext;

namespace ASP_Assignment.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)

        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }


            [Display(Name = "Balance")]
            [RegularExpression(@"^[1-9]\d*(\.\d{2})",
                        ErrorMessage = "Balance should be a number which must include two digits to the right of the decimal.")]
            [Required]
            public decimal Balance { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            var accountTypeNum = Request.Form["AccountType"];
           
            AccountType accountType;
            string acType;
           
            if (accountTypeNum == "0")
            {
                
                accountType = AccountType.Chequing;
                acType = "Chequing";
            }
            else if (accountTypeNum == "1")
            {
                accountType = AccountType.Savings;
                acType = "Savings";
            }
            else
            {
                accountType = AccountType.Savings;
                acType = "Savings";
            }
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    MyRegisteredUser registerUser = new MyRegisteredUser()
                    {
                        Email = Input.Email,
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        AccountType = accountType,
                        Balance = Input.Balance
                    };
                    _context.MyRegisteredUsers.Add(registerUser);
                    _context.SaveChanges();

                    BankAccount bankAccount = new BankAccount()
                    {
                        accountType = acType,
                        balance = Input.Balance,
                    };
                   var ba= _context.BankAccounts.Add(bankAccount);
                    _context.SaveChanges();

                    Client client = new Client()
                    {
                        email = Input.Email,
                        lastName = Input.LastName,
                        firstName = Input.FirstName,

                    };
                    var c=_context.Clients.Add(client);
                    _context.SaveChanges();

                    ClientAccount clientAccount = new ClientAccount()
                    {
                        clientID = c.Entity.clientID,
                        accountNum = ba.Entity.accountNum,
                    };
                    _context.ClientAccounts.Add(clientAccount);
                    _context.SaveChanges();

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Profile", "Accounts", new { email = Input.Email });
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
