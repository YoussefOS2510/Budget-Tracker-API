using Budget_Tracker_API.Data;
using Budget_Tracker_API.DTO;
using Budget_Tracker_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Budget_Tracker_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GoalController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/goal/1/add
        [HttpPost("{userId}/add")]
        public async Task<IActionResult> AddGoal(int userId, [FromBody] GoalDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return BadRequest(new { success = false, message = "User not found." });

            var goal = new Goal
            {
                UserId = userId,
                GoalName = dto.GoalName,
                TargetAmount = dto.TargetAmount,
                CurrentAmount = dto.CurrentAmount,
                Deadline = dto.Deadline
            };

            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Goal added successfully.", goal });
        }

        // GET: api/goal/1
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserGoals(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return BadRequest(new { success = false, message = "User not found." });

            var goals = await _context.Goals
                .Where(g => g.UserId == userId)
                .OrderByDescending(g => g.Deadline)
                .ToListAsync();

            return Ok(new { success = true, goals });
        }

        // PUT: api/goal/1/5/update
        [HttpPut("{userId}/{goalId}/update")]
        public async Task<IActionResult> UpdateGoal(int userId, int goalId, [FromBody] GoalDto dto)
        {
            var goal = await _context.Goals
                .FirstOrDefaultAsync(g => g.UserId == userId && g.GoalId == goalId);

            if (goal == null)
                return NotFound(new { success = false, message = "Goal not found." });

            goal.GoalName = dto.GoalName;
            goal.TargetAmount = dto.TargetAmount;
            goal.CurrentAmount = dto.CurrentAmount;
            goal.Deadline = dto.Deadline;

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Goal updated successfully.", goal });
        }

        // DELETE: api/goal/1/5
        [HttpDelete("{userId}/{goalId}")]
        public async Task<IActionResult> DeleteGoal(int userId, int goalId)
        {
            var goal = await _context.Goals
                .FirstOrDefaultAsync(g => g.UserId == userId && g.GoalId == goalId);

            if (goal == null)
                return NotFound(new { success = false, message = "Goal not found." });

            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Goal deleted successfully." });
        }
    }
}
