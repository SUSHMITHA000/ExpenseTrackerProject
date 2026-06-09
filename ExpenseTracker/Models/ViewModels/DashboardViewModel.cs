using ExpenseTracker.Models;

namespace ExpenseTracker.Models.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TotalExpense { get; set; }

        public int TotalTransactions { get; set; }

        public decimal MonthlyExpense { get; set; }

        public string TopCategory { get; set; } = "";

        public List<Expense> RecentExpenses { get; set; }
            = new();

        public List<string> MonthlyLabels { get; set; } = new();

        public List<decimal> MonthlyAmounts { get; set; } = new();

        public List<string> CategoryLabels { get; set; } = new();

        public List<decimal> CategoryAmounts { get; set; } = new();
    }
}