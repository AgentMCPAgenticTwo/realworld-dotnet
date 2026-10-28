using CleanArchitecture.Domain.Categories;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Categories.Queries.ListCategories;

public record ListCategoriesQuery() : IRequest<ErrorOr<List<Category>>>;
