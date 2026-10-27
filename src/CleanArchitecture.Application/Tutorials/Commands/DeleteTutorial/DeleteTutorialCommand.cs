using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Tutorials.Commands.DeleteTutorial;

public record DeleteTutorialCommand(Guid TutorialId) : IRequest<ErrorOr<Success>>;
