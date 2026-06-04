using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PasswordHash = model.Password
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                return Content("Registration Successful and Saved");
            }

            return View(model);
        }
    }
}