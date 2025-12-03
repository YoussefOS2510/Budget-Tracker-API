using Budget_Tracker_API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Budget_Tracker_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/report/1
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserReport(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Incomes)
                .Include(u => u.Expenses)
                .Include(u => u.Budgets)
                .Include(u => u.Goals)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                return NotFound(new { success = false, message = "User not found." });

            // Total income
            decimal totalIncome = user.Incomes?.Sum(i => i.Amount) ?? 0;

            // Total expenses
            decimal totalExpenses = user.Expenses?.Sum(e => e.Amount) ?? 0;

            // Budget remaining per category (latest budget for each month/year)
            var budgets = await _context.Budgets
                .Where(b => b.UserId == userId)
                .ToListAsync();

            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .Include(e => e.Category)
                .ToListAsync();

            // Group expenses by category
            var expensesByCategory = expenses
                .GroupBy(e => e.Category?.Name ?? "Uncategorized")
                .Select(g => new
                {
                    Category = g.Key,
                    TotalSpent = g.Sum(e => e.Amount)
                }).ToList();

            // Goals progress
            var goalsProgress = user.Goals?.Select(g => new
            {
                g.GoalName,
                g.TargetAmount,
                g.CurrentAmount,
                ProgressPercent = g.TargetAmount > 0 ? (g.CurrentAmount / g.TargetAmount) * 100 : 0
            }).ToList();

            return Ok(new
            {
                success = true,
                totalIncome,
                totalExpenses,
                expensesByCategory,
                goalsProgress
            });
        }
    }
}
