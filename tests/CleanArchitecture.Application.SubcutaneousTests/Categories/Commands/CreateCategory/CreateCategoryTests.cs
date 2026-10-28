using TestCommon.Categories;

namespace CleanArchitecture.Application.SubcutaneousTests.Categories.Commands.CreateCategory;

[Collection(WebAppFactoryCollection.CollectionName)]
public class CreateCategoryTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task CreateCategory_WhenValidCommand_ShouldCreateCategory()
    {
        // Arrange
        var command = CategoryCommandFactory.CreateCreateCategoryCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Name.Should().Be(command.Name);
        result.Value.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateCategory_WhenNameIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        var command = CategoryCommandFactory.CreateCreateCategoryCommand(name: string.Empty);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
    }
}
