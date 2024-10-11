using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Models
{
    public class ExpenseDBContext : DbContext
    {
        public ExpenseDBContext(DbContextOptions options) : base(options) 
        {
            
        }

        public DbSet<Expense> Expenses { get; set; }
    }
}
