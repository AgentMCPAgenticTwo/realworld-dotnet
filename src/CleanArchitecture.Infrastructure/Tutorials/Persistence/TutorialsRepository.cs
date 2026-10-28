using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Tutorials;
using CleanArchitecture.Infrastructure.Common;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Tutorials.Persistence;

public class TutorialsRepository(AppDbContext _dbContext) : ITutorialsRepository
{
    public async Task AddAsync(Tutorial tutorial, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(tutorial, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Tutorial?> GetByIdAsync(Guid tutorialId, CancellationToken cancellationToken)
    {
        return await _dbContext.Tutorials.FirstOrDefaultAsync(
            tutorial => tutorial.Id == tutorialId,
            cancellationToken);
    }

    public async Task<List<Tutorial>> ListAsync(string? title, CancellationToken cancellationToken)
    {
        var query = _dbContext.Tutorials.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(tutorial => tutorial.Title.Contains(title));
        }

        return await query
            .OrderBy(tutorial => tutorial.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task RemoveAsync(Tutorial tutorial, CancellationToken cancellationToken)
    {
        _dbContext.Remove(tutorial);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> RemoveAllAsync(CancellationToken cancellationToken)
    {
        var tutorials = await _dbContext.Tutorials.ToListAsync(cancellationToken);

        _dbContext.RemoveRange(tutorials);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return tutorials.Count;
    }

    public async Task UpdateAsync(Tutorial tutorial, CancellationToken cancellationToken)
    {
        _dbContext.Update(tutorial);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
