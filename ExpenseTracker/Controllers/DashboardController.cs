using ExpenseTracker.Data;
using ExpenseTracker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            //Protect Dashboard
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account");
            }

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

            

            model.RecentExpenses = _context.Expenses
                .Include(x => x.Category)
                .OrderByDescending(x => x.ExpenseDate)
                .Take(5)
                .ToList();

            //Monthly Spending Data

            var monthlyData = _context.Expenses
    .GroupBy(x => new
    {
        x.ExpenseDate.Year,
        x.ExpenseDate.Month
    })
    .OrderBy(x => x.Key.Year)
    .ThenBy(x => x.Key.Month)
    .Select(g => new
    {
        Month =
            new DateTime(
                g.Key.Year,
                g.Key.Month,
                1)
            .ToString("MMM yyyy"),

        Total = g.Sum(x => x.Amount)
    })
    .ToList();

            model.MonthlyLabels =
                monthlyData.Select(x => x.Month).ToList();

            model.MonthlyAmounts =
                monthlyData.Select(x => x.Total).ToList();


            //Category Breakdown Data
            var categoryData = _context.Expenses
    .Include(x => x.Category)
    .GroupBy(x => x.Category.CategoryName)
    .Select(g => new
    {
        Category = g.Key,
        Total = g.Sum(x => x.Amount)
    })
    .ToList();

            model.CategoryLabels =
                categoryData.Select(x => x.Category).ToList();

            model.CategoryAmounts =
                categoryData.Select(x => x.Total).ToList();

            return View(model);
        }
    }
}