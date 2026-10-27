using CleanArchitecture.Domain.Tutorials;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Tutorials.Queries.ListTutorials;

public record ListTutorialsQuery(string? Title) : IRequest<ErrorOr<List<Tutorial>>>;
