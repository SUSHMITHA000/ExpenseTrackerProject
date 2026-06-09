using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models.ViewModels
{
    public class ExpenseViewModel
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string SelectedCategory { get; set; }
    }
}
