using Budget_Tracker_API.Model;
using Microsoft.EntityFrameworkCore;

namespace Budget_Tracker_API.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetCategory> BudgetCategories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> IncomeRecords { get; set; }
        public DbSet<Goal> Goals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // USER → BUDGET one-to-many
            modelBuilder.Entity<Budget>()
                .HasOne(b => b.User)
                .WithMany(u => u.Budgets)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // BUDGET → BUDGETCATEGORY one-to-many
            modelBuilder.Entity<BudgetCategory>()
                .HasOne(c => c.Budget)
                .WithMany(b => b.Categories)
                .HasForeignKey(c => c.BudgetId)
                .OnDelete(DeleteBehavior.Cascade);

            // BUDGETCATEGORY → EXPENSE one-to-many
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // USER → EXPENSE one-to-many
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.User)
                .WithMany(u => u.Expenses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // USER → INCOME one-to-many
            modelBuilder.Entity<Income>()
                .HasOne(i => i.User)
                .WithMany(u => u.IncomeRecords)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // USER → GOAL one-to-many
            modelBuilder.Entity<Goal>()
                .HasOne(g => g.User)
                .WithMany(u => u.Goals)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
