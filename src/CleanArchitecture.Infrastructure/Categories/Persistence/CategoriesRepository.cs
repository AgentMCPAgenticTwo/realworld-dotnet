using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Categories;
using CleanArchitecture.Infrastructure.Common;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Categories.Persistence;

public class CategoriesRepository(AppDbContext _dbContext) : ICategoriesRepository
{
    public async Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(category, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _dbContext.Categories.FirstOrDefaultAsync(
            category => category.Id == categoryId,
            cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _dbContext.Categories.AnyAsync(
            category => category.Id == categoryId,
            cancellationToken);
    }

    public async Task<List<Category>> ListAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .ToListAsync(cancellationToken);
    }
}
