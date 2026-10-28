using CleanArchitecture.Domain.Categories;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface ICategoriesRepository
{
    Task AddAsync(Category category, CancellationToken cancellationToken);
    Task<Category?> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid categoryId, CancellationToken cancellationToken);
    Task<List<Category>> ListAsync(CancellationToken cancellationToken);
}
