using Microsoft.EntityFrameworkCore;
using TradeCategorizationSystem.Domain;

namespace TradeCategorizationSystem.Infrastructure
{
    public class TradeCategoryDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public TradeCategoryDbContext(DbContextOptions<TradeCategoryDbContext> options) : base(options)
        {
            //todo
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }

    }
}
