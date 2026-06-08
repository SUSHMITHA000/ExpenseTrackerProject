using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpenseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Categories = new SelectList(
                _context.Categories.ToList(),
                "CategoryId",
                "CategoryName");

            return View();
        }

        [HttpPost]
        public IActionResult Index(ExpenseViewModel model)
        {
            if (ModelState.IsValid)
            {
                Expense expense = new Expense
                {
                    Amount = model.Amount,
                    Description = model.Description,
                    ExpenseDate = model.ExpenseDate.ToUniversalTime(),
                    CreatedDate = DateTime.UtcNow,

                    // Temporary user id
                    UserId = 1,

                    CategoryId = model.CategoryId
                };

                _context.Expenses.Add(expense);
                _context.SaveChanges();

                return Content("Expense Saved Successfully");
            }

            ViewBag.Categories = new SelectList(
                _context.Categories.ToList(),
                "CategoryId",
                "CategoryName");

            return View(model);
        }
        public IActionResult List()
        {
            var expenses = _context.Expenses
                .Include(e => e.Category)
                .OrderByDescending(e => e.ExpenseDate)
                .ToList();

            return View(expenses);
        }
        public IActionResult Edit(int id)
        {
            var expense = _context.Expenses.Find(id);

            if (expense == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(
                _context.Categories.ToList(),
                "CategoryId",
                "CategoryName");

            return View(expense);
        }
        [HttpPost]
        public IActionResult Edit(Expense expense)
        {
            var existingExpense = _context.Expenses.Find(expense.ExpenseId);

            if (existingExpense == null)
            {
                return NotFound();
            }

            existingExpense.Amount = expense.Amount;
            existingExpense.Description = expense.Description;

            _context.SaveChanges();

            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            var expense = _context.Expenses.Find(id);

            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            _context.SaveChanges();

            return RedirectToAction("List");
        }
    }
}