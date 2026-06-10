using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

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

            ViewBag.MonthlyTotal =
                _context.Expenses
                    .ToList()
                    .Where(x =>
                        x.ExpenseDate.Month == DateTime.UtcNow.Month &&
                        x.ExpenseDate.Year == DateTime.UtcNow.Year)
                    .Sum(x => x.Amount);

            ViewBag.TodayTotal =
                _context.Expenses
                    .ToList()
                    .Where(x =>
                        x.ExpenseDate.Date == DateTime.UtcNow.Date)
                    .Sum(x => x.Amount);

            ViewBag.TopCategory =
                _context.Expenses
                    .ToList()
                    .GroupBy(x => x.CategoryId)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.First().Category?.CategoryName)
                    .FirstOrDefault() ?? "N/A";

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
                    UserId = 1,
                    CategoryId = model.CategoryId
                };

                _context.Expenses.Add(expense);
                _context.SaveChanges();

                return RedirectToAction("List");
            }

            ViewBag.Categories = new SelectList(
                _context.Categories.ToList(),
                "CategoryId",
                "CategoryName");

            return View(model);
        }

        public IActionResult List(
            string searchText,
            int? categoryId,
            DateTime? fromDate,
            DateTime? toDate)
        {
            var expenses = _context.Expenses
                .Include(e => e.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                expenses = expenses.Where(e =>
                    e.Description.Contains(searchText));
            }

            if (categoryId.HasValue)
            {
                expenses = expenses.Where(e =>
                    e.CategoryId == categoryId);
            }

            if (fromDate.HasValue)
            {
                var fromUtc =
                    DateTime.SpecifyKind(
                        fromDate.Value,
                        DateTimeKind.Utc);

                expenses = expenses.Where(e =>
                    e.ExpenseDate >= fromUtc);
            }

            if (toDate.HasValue)
            {
                var toUtc =
                    DateTime.SpecifyKind(
                        toDate.Value.AddDays(1),
                        DateTimeKind.Utc);

                expenses = expenses.Where(e =>
                    e.ExpenseDate < toUtc);
            }


            ViewBag.Categories = new SelectList(
    _context.Categories,
    "CategoryId",
    "CategoryName");

            var expenseList = expenses.ToList();

            ViewBag.TotalExpenses =
                expenseList.Count;

            ViewBag.TotalAmount =
                expenseList.Sum(x => x.Amount);

            ViewBag.HighestExpense =
                expenseList.Any()
                    ? expenseList.Max(x => x.Amount)
                    : 0;

            var highestExpenseRecord = expenseList
    .OrderByDescending(x => x.Amount)
    .FirstOrDefault();

            ViewBag.HighestExpenseCategory =
                highestExpenseRecord?.Category?.CategoryName ?? "N/A";

            ViewBag.AverageExpense =
                expenseList.Any()
                    ? expenseList.Average(x => x.Amount)
                    : 0;

            return View(
                expenseList
                    .OrderByDescending(e => e.ExpenseDate)
                    .ToList());


        }

        public IActionResult Edit(int id)
        {
            var expense = _context.Expenses.Find(id);

            if (expense == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(
                _context.Categories,
                "CategoryId",
                "CategoryName",
                expense.CategoryId);

            return View(expense);
        }

        [HttpPost]
        public IActionResult Edit(Expense expense)
        {
            var existingExpense =
                _context.Expenses.Find(expense.ExpenseId);

            if (existingExpense == null)
            {
                return NotFound();
            }

            existingExpense.Amount = expense.Amount;
            existingExpense.Description = expense.Description;
            existingExpense.CategoryId = expense.CategoryId;
            existingExpense.ExpenseDate =
                    DateTime.SpecifyKind(
                        expense.ExpenseDate,
                        DateTimeKind.Utc);

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

