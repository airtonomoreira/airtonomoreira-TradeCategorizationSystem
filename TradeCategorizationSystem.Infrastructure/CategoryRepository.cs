using Microsoft.EntityFrameworkCore;
using TradeCategorizationSystem.Domain;

namespace TradeCategorizationSystem.Infrastructure;

public class CategoryRepository
{
    private readonly TradeCategoryDbContext _context;

    public CategoryRepository(TradeCategoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> GetCategoryByNameAsync(string name)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<Category> GetCategoryByIdAsync(string id)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToString() == id);
    }

    public async Task AddCategoryAsync(Category category)
    {
        if (await _context.Categories.AnyAsync(c => c.Name == category.Name))
        {
            throw new InvalidOperationException("A category with the same name already exists.");
        }

        if (await _context.Categories.AnyAsync(c =>
            c.ClientSector == category.ClientSector &&
            ((category.InitialValue >= c.InitialValue && category.InitialValue <= c.FinalValue) ||
             (category.FinalValue >= c.InitialValue && category.FinalValue <= c.FinalValue))))
        {
            throw new InvalidOperationException("Overlapping value ranges for the same client sector are not allowed.");
        }

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckForOverlappingRangesAsync(Category category)
    {
        return await _context.Categories.AnyAsync(c =>
            c.ClientSector == category.ClientSector &&
            ((category.InitialValue >= c.InitialValue && category.InitialValue <= c.FinalValue) ||
             (category.FinalValue >= c.InitialValue && category.FinalValue <= c.FinalValue)));
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(string name)
    {
        var category = await GetCategoryByNameAsync(name);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
