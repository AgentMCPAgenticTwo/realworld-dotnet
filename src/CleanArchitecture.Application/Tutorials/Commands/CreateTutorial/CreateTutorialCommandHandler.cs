using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Tutorials;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Tutorials.Commands.CreateTutorial;

public class CreateTutorialCommandHandler(
    ITutorialsRepository _tutorialsRepository,
    IDateTimeProvider _dateTimeProvider) : IRequestHandler<CreateTutorialCommand, ErrorOr<Tutorial>>
{
    public async Task<ErrorOr<Tutorial>> Handle(CreateTutorialCommand command, CancellationToken cancellationToken)
    {
        var tutorial = new Tutorial(
            command.Title,
            command.Description,
            command.Published,
            _dateTimeProvider.UtcNow);

        await _tutorialsRepository.AddAsync(tutorial, cancellationToken);

        return tutorial;
    }
}
