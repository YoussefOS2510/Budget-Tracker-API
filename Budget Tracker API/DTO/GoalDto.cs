using System.ComponentModel.DataAnnotations;

namespace Budget_Tracker_API.DTO
{
    public class GoalDto
    {
        [Required]
        [MaxLength(150)]
        public string GoalName { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 999999)]
        public decimal TargetAmount { get; set; }

        public decimal CurrentAmount { get; set; } = 0;

        public DateTime? Deadline { get; set; }
    }
}
