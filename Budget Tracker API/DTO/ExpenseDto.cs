using System.ComponentModel.DataAnnotations;

namespace Budget_Tracker_API.DTO
{
    public class ExpenseDto
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime ExpenseDate { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public bool IsRecurring { get; set; } = false;
    }
}
