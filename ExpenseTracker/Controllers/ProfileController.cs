using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var user = _context.Users.FirstOrDefault();

            return View(user);
        }
    }
}