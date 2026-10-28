using TestCommon.Categories;
using TestCommon.Tutorials;

namespace CleanArchitecture.Application.SubcutaneousTests.Tutorials.Commands.CreateTutorial;

[Collection(WebAppFactoryCollection.CollectionName)]
public class CreateTutorialTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task CreateTutorial_WhenValidCommand_ShouldCreateTutorial()
    {
        // Arrange
        var command = TutorialCommandFactory.CreateCreateTutorialCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Title.Should().Be(command.Title);
        result.Value.Description.Should().Be(command.Description);
        result.Value.Published.Should().Be(command.Published);
        result.Value.UpdatedAt.Should().Be(result.Value.CreatedAt);
    }

    [Fact]
    public async Task CreateTutorial_WhenCategoryExists_ShouldAssignTheCategory()
    {
        // Arrange
        var category = (await _mediator.Send(CategoryCommandFactory.CreateCreateCategoryCommand())).Value;
        var command = TutorialCommandFactory.CreateCreateTutorialCommand(categoryId: category.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.CategoryId.Should().Be(category.Id);
    }

    [Fact]
    public async Task CreateTutorial_WhenCategoryDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var command = TutorialCommandFactory.CreateCreateTutorialCommand(categoryId: Guid.NewGuid());

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task CreateTutorial_WhenTitleIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        var command = TutorialCommandFactory.CreateCreateTutorialCommand(title: string.Empty);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
    }
}
