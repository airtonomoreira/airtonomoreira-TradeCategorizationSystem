using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace TradeCategorizationSystem.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TradeCategoryDbContext>
    {
        public TradeCategoryDbContext CreateDbContext(string[] args = null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TradeCategoryDbContext>();
            optionsBuilder.UseSqlite("Data Source=tradecategories.db");

            return new TradeCategoryDbContext(optionsBuilder.Options);
        }
    }
}
