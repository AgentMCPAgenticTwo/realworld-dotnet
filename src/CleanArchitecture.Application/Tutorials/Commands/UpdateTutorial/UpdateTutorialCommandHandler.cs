using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Tutorials;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Tutorials.Commands.UpdateTutorial;

public class UpdateTutorialCommandHandler(
    ITutorialsRepository _tutorialsRepository,
    ICategoriesRepository _categoriesRepository,
    IDateTimeProvider _dateTimeProvider) : IRequestHandler<UpdateTutorialCommand, ErrorOr<Tutorial>>
{
    public async Task<ErrorOr<Tutorial>> Handle(UpdateTutorialCommand command, CancellationToken cancellationToken)
    {
        var tutorial = await _tutorialsRepository.GetByIdAsync(command.TutorialId, cancellationToken);

        if (tutorial is null)
        {
            return Error.NotFound(description: "Tutorial not found");
        }

        if (command.CategoryId is not null &&
            !await _categoriesRepository.ExistsAsync(command.CategoryId.Value, cancellationToken))
        {
            return Error.NotFound(description: "Category not found");
        }

        tutorial.Update(
            command.Title,
            command.Description,
            command.Published,
            command.CategoryId,
            _dateTimeProvider.UtcNow);

        await _tutorialsRepository.UpdateAsync(tutorial, cancellationToken);

        return tutorial;
    }
}
