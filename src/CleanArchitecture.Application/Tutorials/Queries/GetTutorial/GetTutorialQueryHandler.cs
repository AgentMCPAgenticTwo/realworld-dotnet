using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Tutorials;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Tutorials.Queries.GetTutorial;

public class GetTutorialQueryHandler(ITutorialsRepository _tutorialsRepository)
    : IRequestHandler<GetTutorialQuery, ErrorOr<Tutorial>>
{
    public async Task<ErrorOr<Tutorial>> Handle(GetTutorialQuery query, CancellationToken cancellationToken)
    {
        var tutorial = await _tutorialsRepository.GetByIdAsync(query.TutorialId, cancellationToken);

        if (tutorial is null)
        {
            return Error.NotFound(description: "Tutorial not found");
        }

        return tutorial;
    }
}
