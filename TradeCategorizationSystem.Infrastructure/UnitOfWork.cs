using System;
using System.Threading.Tasks;
using TradeCategorizationSystem.Domain;
using TradeCategorizationSystem.Infrastructure;
namespace TradeCategorizationSystem.Infrastructure
{
    public class UnitOfWork : IDisposable
    {
        private readonly TradeCategoryDbContext _context;
        private CategoryRepository _categoryRepository;

        public UnitOfWork(TradeCategoryDbContext context)
        {
            _context = context;
        }

        public CategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository ??= new CategoryRepository(_context);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
