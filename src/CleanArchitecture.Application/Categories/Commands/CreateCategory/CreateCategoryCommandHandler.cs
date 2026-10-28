using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Categories;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler(ICategoriesRepository _categoriesRepository)
    : IRequestHandler<CreateCategoryCommand, ErrorOr<Category>>
{
    public async Task<ErrorOr<Category>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = new Category(command.Name);

        await _categoriesRepository.AddAsync(category, cancellationToken);

        return category;
    }
}
