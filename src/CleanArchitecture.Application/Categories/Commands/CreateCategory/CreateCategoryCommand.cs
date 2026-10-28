using CleanArchitecture.Domain.Categories;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string Name) : IRequest<ErrorOr<Category>>;
