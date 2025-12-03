using Microsoft.EntityFrameworkCore;
using Budget_Tracker_API.Model;

namespace Budget_Tracker_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<BudgetCategory> BudgetCategories { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Goal> Goals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------------------
            // USER → INCOMES (1-many)
            // ---------------------------
            modelBuilder.Entity<Income>()
                .HasOne(i => i.User)
                .WithMany(u => u.Incomes)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------
            // USER → EXPENSES (1-many)
            // ---------------------------
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.User)
                .WithMany(u => u.Expenses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------
            // USER → GOALS (1-many)
            // ---------------------------
            modelBuilder.Entity<Goal>()
                .HasOne(g => g.User)
                .WithMany(u => u.Goals)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------
            // USER → BUDGETS (1-many)
            // ---------------------------
            modelBuilder.Entity<Budget>()
                .HasOne(b => b.User)
                .WithMany(u => u.Budgets)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------
            // USER → CUSTOM CATEGORIES (1-many)
            // UserId = null → preset category
            // ---------------------------
            modelBuilder.Entity<BudgetCategory>()
                .HasOne(c => c.User)
                .WithMany(u => u.BudgetCategories)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------
            // CATEGORY → EXPENSES (1-many)
            // ---------------------------
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------
            // PRESET CATEGORY SEEDING
            // ---------------------------
            modelBuilder.Entity<BudgetCategory>().HasData(
                new BudgetCategory { CategoryId = 1, Name = "Food", UserId = null },
                new BudgetCategory { CategoryId = 2, Name = "Fun", UserId = null },
                new BudgetCategory { CategoryId = 3, Name = "Shopping", UserId = null },
                new BudgetCategory { CategoryId = 4, Name = "Health", UserId = null },
                new BudgetCategory { CategoryId = 5, Name = "Transportation", UserId = null },
                new BudgetCategory { CategoryId = 6, Name = "Bills", UserId = null },
                new BudgetCategory { CategoryId = 7, Name = "Other", UserId = null }
            );
        }
    }
}
