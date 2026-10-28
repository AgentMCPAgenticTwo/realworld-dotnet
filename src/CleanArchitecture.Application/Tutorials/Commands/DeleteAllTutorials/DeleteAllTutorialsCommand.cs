using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Tutorials.Commands.DeleteAllTutorials;

public record DeleteAllTutorialsCommand : IRequest<ErrorOr<int>>;
