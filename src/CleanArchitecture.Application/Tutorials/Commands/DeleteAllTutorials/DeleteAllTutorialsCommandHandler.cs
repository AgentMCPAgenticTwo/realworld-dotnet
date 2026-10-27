using CleanArchitecture.Application.Common.Interfaces;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Tutorials.Commands.DeleteAllTutorials;

public class DeleteAllTutorialsCommandHandler(ITutorialsRepository _tutorialsRepository)
    : IRequestHandler<DeleteAllTutorialsCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(DeleteAllTutorialsCommand command, CancellationToken cancellationToken)
    {
        return await _tutorialsRepository.RemoveAllAsync(cancellationToken);
    }
}
