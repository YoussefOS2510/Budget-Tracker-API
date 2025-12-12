using Budget_Tracker_API.Data;
using Budget_Tracker_API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Budget_Tracker_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetCategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BudgetCategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/BudgetCategory/preset
        [HttpGet("preset")]
        public async Task<IActionResult> GetPresetCategories()
        {
            var categories = await _context.BudgetCategories
                .Where(c => c.UserId == null)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return Ok(categories);
        }
        
    }
}
