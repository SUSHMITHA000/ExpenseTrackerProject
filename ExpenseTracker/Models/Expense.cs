using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public DateTime ExpenseDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}