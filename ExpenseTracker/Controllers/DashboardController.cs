using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TotalUsers = _context.Users.Count();

            ViewBag.TotalExpenses = _context.Expenses.Count();

            ViewBag.TotalCategories = _context.Categories.Count();

            ViewBag.TotalAmount =
                _context.Expenses.Sum(e => (decimal?)e.Amount) ?? 0;

            return View();
        }
    }
}