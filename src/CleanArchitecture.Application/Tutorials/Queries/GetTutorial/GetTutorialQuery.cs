using CleanArchitecture.Domain.Tutorials;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Tutorials.Queries.GetTutorial;

public record GetTutorialQuery(Guid TutorialId) : IRequest<ErrorOr<Tutorial>>;
