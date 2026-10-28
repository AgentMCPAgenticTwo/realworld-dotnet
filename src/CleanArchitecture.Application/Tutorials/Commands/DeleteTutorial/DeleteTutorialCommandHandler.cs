using CleanArchitecture.Application.Common.Interfaces;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Tutorials.Commands.DeleteTutorial;

public class DeleteTutorialCommandHandler(ITutorialsRepository _tutorialsRepository)
    : IRequestHandler<DeleteTutorialCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(DeleteTutorialCommand command, CancellationToken cancellationToken)
    {
        var tutorial = await _tutorialsRepository.GetByIdAsync(command.TutorialId, cancellationToken);

        if (tutorial is null)
        {
            return Error.NotFound(description: "Tutorial not found");
        }

        await _tutorialsRepository.RemoveAsync(tutorial, cancellationToken);

        return Result.Success;
    }
}
