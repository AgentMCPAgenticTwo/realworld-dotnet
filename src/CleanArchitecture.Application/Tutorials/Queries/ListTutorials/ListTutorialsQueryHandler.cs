using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Tutorials;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Tutorials.Queries.ListTutorials;

public class ListTutorialsQueryHandler(ITutorialsRepository _tutorialsRepository)
    : IRequestHandler<ListTutorialsQuery, ErrorOr<List<Tutorial>>>
{
    public async Task<ErrorOr<List<Tutorial>>> Handle(ListTutorialsQuery query, CancellationToken cancellationToken)
    {
        return await _tutorialsRepository.ListAsync(query.Title, cancellationToken);
    }
}
