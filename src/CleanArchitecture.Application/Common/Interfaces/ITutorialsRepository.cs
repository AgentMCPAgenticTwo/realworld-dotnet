using CleanArchitecture.Domain.Tutorials;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface ITutorialsRepository
{
    Task AddAsync(Tutorial tutorial, CancellationToken cancellationToken);
    Task<Tutorial?> GetByIdAsync(Guid tutorialId, CancellationToken cancellationToken);
    Task<List<Tutorial>> ListAsync(string? title, CancellationToken cancellationToken);
    Task RemoveAsync(Tutorial tutorial, CancellationToken cancellationToken);
    Task<int> RemoveAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(Tutorial tutorial, CancellationToken cancellationToken);
}
