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
    public class IncomeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IncomeController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/income/1/add
        [HttpPost("{userId}/add")]
        public async Task<IActionResult> AddIncome(int userId, [FromBody] IncomeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return BadRequest(new { message = "User not found." });

            var income = new Income
            {
                UserId = userId,
                Amount = dto.Amount,
                SourceName = dto.SourceName,
                IncomeDate = dto.IncomeDate,
                IsRecurring = dto.IsRecurring // defaults to false anyway
            };

            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Income added successfully.", income });
        }

        // GET: api/income/1
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserIncomes(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return BadRequest(new { message = "User not found." });

            var incomes = await _context.Incomes
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.IncomeDate)
                .ToListAsync();

            return Ok(incomes);
        }

        // PUT: api/income/1/5/update
        [HttpPut("{userId}/{incomeId}/update")]
        public async Task<IActionResult> UpdateIncome(int userId, int incomeId, [FromBody] IncomeDto dto)
        {
            var income = await _context.Incomes
                .FirstOrDefaultAsync(i => i.UserId == userId && i.IncomeId == incomeId);

            if (income == null)
                return NotFound(new { success = false, message = "Income not found." });

            income.SourceName = dto.SourceName;
            income.Amount = dto.Amount;
            income.IncomeDate = dto.IncomeDate;
            income.IsRecurring = dto.IsRecurring;

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Income updated successfully.", income });
        }

        // DELETE: api/income/1/5
        [HttpDelete("{userId}/{incomeId}")]
        public async Task<IActionResult> DeleteIncome(int userId, int incomeId)
        {
            var income = await _context.Incomes
                .FirstOrDefaultAsync(i => i.UserId == userId && i.IncomeId == incomeId);

            if (income == null)
                return NotFound(new { success = false, message = "Income not found." });

            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Income deleted successfully." });
        }

    }
}
