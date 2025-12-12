using Budget_Tracker_API.Data;
using Budget_Tracker_API.DTO;
using Budget_Tracker_API.Model;
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

        // POST: api/budget/{userId}/add
        [HttpPost("{userId}/add")]
        public async Task<IActionResult> AddBudget(int userId, [FromBody] BudgetDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found" });

            var category = await _context.BudgetCategories.FindAsync(dto.CategoryId);
            if (category == null)
                return NotFound(new { message = "Category not found" });

            var budget = new Budget
            {
                UserId = userId,
                Month = dto.Month,
                Year = dto.Year,
                Amount = dto.Amount,
                Categories = new List<BudgetCategory> { category } // one category
            };

            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Budget created", budgetId = budget.BudgetId });
        }

        // GET: api/budget/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserBudgets(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return BadRequest(new { success = false, message = "User not found." });

            var budgets = await _context.Budgets
                .Include(b => b.Categories)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.Year)
                .ThenByDescending(b => b.Month)
                .ToListAsync();

            // Return the single category per budget
            var result = budgets.Select(b => new
            {
                b.BudgetId,
                b.Month,
                b.Year,
                b.Amount,
                Category = b.Categories?.FirstOrDefault()
            });

            return Ok(new { success = true, budgets = result });
        }

        // GET: api/budget/details/{id}
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetBudget(int id)
        {
            var budget = await _context.Budgets
                .Include(b => b.Categories)
                .FirstOrDefaultAsync(b => b.BudgetId == id);

            if (budget == null)
                return NotFound();

            return Ok(new
            {
                budget.BudgetId,
                budget.Month,
                budget.Year,
                budget.Amount,
                Category = budget.Categories?.FirstOrDefault(),
                budget.CreatedAt
            });
        }

        // PUT: api/budget/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBudget(int id, [FromBody] BudgetDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var budget = await _context.Budgets
                .Include(b => b.Categories)
                .FirstOrDefaultAsync(b => b.BudgetId == id);

            if (budget == null)
                return NotFound();

            var category = await _context.BudgetCategories.FindAsync(dto.CategoryId);
            if (category == null)
                return NotFound(new { message = "Category not found" });

            // Update fields
            budget.Month = dto.Month;
            budget.Year = dto.Year;
            budget.Amount = dto.Amount;

            // Replace the single category
            budget.Categories?.Clear();
            budget.Categories = new List<BudgetCategory> { category };

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Budget updated" });
        }

        // DELETE: api/budget/{userId}/{budgetId}
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
