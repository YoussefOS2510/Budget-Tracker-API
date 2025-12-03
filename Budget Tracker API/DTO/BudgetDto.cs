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
    }
}
