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
    public class ExpenseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExpenseController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/expense/1/add
        [HttpPost("{userId}/add")]
        public async Task<IActionResult> AddExpense(int userId, [FromBody] ExpenseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate User
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return BadRequest(new { success = false, message = "User not found." });

            // Validate Category
            var category = await _context.BudgetCategories.FindAsync(dto.CategoryId);
            if (category == null)
                return BadRequest(new { success = false, message = "Category not found." });

            var expense = new Expense
            {
                UserId = userId,
                Amount = dto.Amount,
                Description = dto.Description,
                ExpenseDate = dto.ExpenseDate,
                CategoryId = dto.CategoryId,
                IsRecurring = dto.IsRecurring // defaults to false if not sent
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Expense added successfully.", expense });
        }

        // GET: api/expense/1
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserExpenses(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return BadRequest(new { success = false, message = "User not found." });

            var expenses = await _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();

            return Ok(new { success = true, expenses });
        }
    }
}
