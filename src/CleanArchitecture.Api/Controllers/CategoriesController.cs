using CleanArchitecture.Application.Categories.Commands.CreateCategory;
using CleanArchitecture.Application.Categories.Queries.ListCategories;
using CleanArchitecture.Contracts.Categories;
using CleanArchitecture.Domain.Categories;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[AllowAnonymous]
[Route("api/categories")]
public class CategoriesController(ISender _mediator) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> ListCategories()
    {
        var query = new ListCategoriesQuery();

        var result = await _mediator.Send(query);

        return result.Match(
            categories => Ok(categories.ConvertAll(ToDto)),
            Problem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
    {
        var command = new CreateCategoryCommand(request.Name);

        var result = await _mediator.Send(command);

        return result.Match(
            category => CreatedAtAction(
                actionName: nameof(ListCategories),
                routeValues: null,
                value: ToDto(category)),
            Problem);
    }

    private CategoryResponse ToDto(Category category) =>
        new(
            category.Id,
            category.Name);
}
