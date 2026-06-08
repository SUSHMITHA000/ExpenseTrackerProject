using ExpenseTracker.Data;
using ExpenseTracker.Models.ViewModels;
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
            DashboardViewModel model = new DashboardViewModel();

            model.TotalExpense =
                _context.Expenses.Sum(x => (decimal?)x.Amount) ?? 0;

            model.TotalTransactions =
                _context.Expenses.Count();

            model.MonthlyExpense =
                _context.Expenses
                    .Where(x => x.ExpenseDate.Month == DateTime.Now.Month
                             && x.ExpenseDate.Year == DateTime.Now.Year)
                    .Sum(x => (decimal?)x.Amount) ?? 0;

            model.TopCategory =
                _context.Expenses
                    .GroupBy(x => x.CategoryId)
                    .OrderByDescending(g => g.Sum(x => x.Amount))
                    .Select(g => g.First().Category.CategoryName)
                    .FirstOrDefault() ?? "N/A";

            return View(model);
        }
    }
}