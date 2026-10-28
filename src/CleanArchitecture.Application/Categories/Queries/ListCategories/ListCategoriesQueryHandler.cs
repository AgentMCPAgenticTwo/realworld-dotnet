using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Categories;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Categories.Queries.ListCategories;

public class ListCategoriesQueryHandler(ICategoriesRepository _categoriesRepository)
    : IRequestHandler<ListCategoriesQuery, ErrorOr<List<Category>>>
{
    public async Task<ErrorOr<List<Category>>> Handle(ListCategoriesQuery query, CancellationToken cancellationToken)
    {
        return await _categoriesRepository.ListAsync(cancellationToken);
    }
}
