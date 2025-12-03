using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget_Tracker_API.Model
{
    public class User
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(120)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        // Navigation Collections
        public ICollection<Income>? Incomes { get; set; }
        public ICollection<Expense>? Expenses { get; set; }
        public ICollection<BudgetCategory>? BudgetCategories { get; set; }
        public ICollection<Budget>? Budgets { get; set; }
        public ICollection<Goal>? Goals { get; set; }
    }

}
