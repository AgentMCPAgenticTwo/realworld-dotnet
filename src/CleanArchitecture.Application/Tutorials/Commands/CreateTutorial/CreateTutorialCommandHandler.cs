using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Tutorials;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Tutorials.Commands.CreateTutorial;

public class CreateTutorialCommandHandler(
    ITutorialsRepository _tutorialsRepository,
    ICategoriesRepository _categoriesRepository,
    IDateTimeProvider _dateTimeProvider) : IRequestHandler<CreateTutorialCommand, ErrorOr<Tutorial>>
{
    public async Task<ErrorOr<Tutorial>> Handle(CreateTutorialCommand command, CancellationToken cancellationToken)
    {
        if (command.CategoryId is not null &&
            !await _categoriesRepository.ExistsAsync(command.CategoryId.Value, cancellationToken))
        {
            return Error.NotFound(description: "Category not found");
        }

        var tutorial = new Tutorial(
            command.Title,
            command.Description,
            command.Published,
            _dateTimeProvider.UtcNow,
            command.CategoryId);

        await _tutorialsRepository.AddAsync(tutorial, cancellationToken);

        return tutorial;
    }
}
