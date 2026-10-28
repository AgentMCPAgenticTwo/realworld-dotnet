using TestCommon.Categories;

namespace CleanArchitecture.Application.SubcutaneousTests.Categories.Queries.ListCategories;

[Collection(WebAppFactoryCollection.CollectionName)]
public class ListCategoriesTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task ListCategories_WhenCategoriesExist_ShouldReturnThemOrderedByName()
    {
        // Arrange
        await _mediator.Send(CategoryCommandFactory.CreateCreateCategoryCommand(name: "Testing"));
        await _mediator.Send(CategoryCommandFactory.CreateCreateCategoryCommand(name: "Architecture"));

        // Act
        var result = await _mediator.Send(CategoryQueryFactory.CreateListCategoriesQuery());

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().HaveCount(2);
        result.Value[0].Name.Should().Be("Architecture");
        result.Value[1].Name.Should().Be("Testing");
    }
}
