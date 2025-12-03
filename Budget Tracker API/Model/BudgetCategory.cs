using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget_Tracker_API.Model
{
    public class BudgetCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(60)]
        public string Name { get; set; } = string.Empty;

        // NULL → preset category
        public int? UserId { get; set; }

        // Navigation
        public User? User { get; set; }
        public ICollection<Expense>? Expenses { get; set; }
    }
}
