using System.ComponentModel.DataAnnotations;

namespace Budget_Tracker_API.DTO
{
    public class BudgetDto
    {
        [Required]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        [Range(2000, 2100)]
        public int Year { get; set; }

        [Required]
        [Range(0.0, 999999999.99)]
        public decimal Amount { get; set; } = 0m; // <-- new

        [Required]
        public int CategoryId { get; set; }
    }
}
