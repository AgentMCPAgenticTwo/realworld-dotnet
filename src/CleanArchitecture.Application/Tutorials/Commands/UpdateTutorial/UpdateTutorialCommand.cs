using CleanArchitecture.Domain.Tutorials;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Tutorials.Commands.UpdateTutorial;

public record UpdateTutorialCommand(Guid TutorialId, string Title, string Description, bool Published)
    : IRequest<ErrorOr<Tutorial>>;
