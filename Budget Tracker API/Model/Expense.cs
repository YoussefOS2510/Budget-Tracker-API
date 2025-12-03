using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget_Tracker_API.Model
{
    public class Expense
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ExpenseId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        [Required]
        [Range(0.01, 999999)]
        public decimal Amount { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        public bool IsRecurring { get; set; } = false;

        // Navigation
        public User? User { get; set; }
        public BudgetCategory? Category { get; set; }
    }
}
