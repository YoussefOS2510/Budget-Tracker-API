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
    public class BudgetController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BudgetController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/budget/1/add
        [HttpPost("{userId}/add")]
        public async Task<IActionResult> AddBudget(int userId, [FromBody] BudgetDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return BadRequest(new { success = false, message = "User not found." });

            // Prevent duplicate budget for the same month/year
            var existingBudget = await _context.Budgets
                .FirstOrDefaultAsync(b => b.UserId == userId && b.Month == dto.Month && b.Year == dto.Year);

            if (existingBudget != null)
                return Conflict(new { success = false, message = "Budget for this month already exists." });

            var budget = new Budget
            {
                UserId = userId,
                Month = dto.Month,
                Year = dto.Year,
                CreatedAt = DateTime.UtcNow
            };

            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Budget created successfully.", budget });
        }

        // GET: api/budget/1
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserBudgets(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return BadRequest(new { success = false, message = "User not found." });

            var budgets = await _context.Budgets
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.Year)
                .ThenByDescending(b => b.Month)
                .ToListAsync();

            return Ok(new { success = true, budgets });
        }

        // GET: api/budget/1/5
        [HttpGet("{userId}/{budgetId}")]
        public async Task<IActionResult> GetBudgetById(int userId, int budgetId)
        {
            var budget = await _context.Budgets
                .FirstOrDefaultAsync(b => b.UserId == userId && b.BudgetId == budgetId);

            if (budget == null)
                return NotFound(new { success = false, message = "Budget not found." });

            return Ok(new { success = true, budget });
        }

        // DELETE: api/budget/1/5
        [HttpDelete("{userId}/{budgetId}")]
        public async Task<IActionResult> DeleteBudget(int userId, int budgetId)
        {
            var budget = await _context.Budgets
                .FirstOrDefaultAsync(b => b.UserId == userId && b.BudgetId == budgetId);

            if (budget == null)
                return NotFound(new { success = false, message = "Budget not found." });

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Budget deleted successfully." });
        }
    }
}
