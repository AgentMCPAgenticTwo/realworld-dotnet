using CleanArchitecture.Domain.Tutorials;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Tutorials.Commands.CreateTutorial;

public record CreateTutorialCommand(string Title, string Description, bool Published, Guid? CategoryId = null)
    : IRequest<ErrorOr<Tutorial>>;
