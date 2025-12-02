using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget_Tracker_API.Model
{
    public class User
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ICollection<Budget>? Budgets { get; set; }
        public ICollection<Expense>? Expenses { get; set; }
        public ICollection<Income>? IncomeRecords { get; set; }
        public ICollection<Goal>? Goals { get; set; }

    }
}
