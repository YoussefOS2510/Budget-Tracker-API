using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget_Tracker_API.Model
{
    public class Budget
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int BudgetId { get; set; }

        [Required]
        public int UserId { get; set; }      // Foreign Key

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }       // 1–12

        [Required]
        [Range(2000, 2100)]
        public int Year { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.0, 999999999.99)]
        public decimal Amount { get; set; } = 0m; // <-- new

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User? User { get; set; }
        public ICollection<BudgetCategory>? Categories { get; set; }
    }
}
