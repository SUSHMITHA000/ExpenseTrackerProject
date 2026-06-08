namespace ExpenseTracker.Models.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TotalExpense { get; set; }

        public int TotalTransactions { get; set; }

        public decimal MonthlyExpense { get; set; }

        public string TopCategory { get; set; } = "";
    }
}
