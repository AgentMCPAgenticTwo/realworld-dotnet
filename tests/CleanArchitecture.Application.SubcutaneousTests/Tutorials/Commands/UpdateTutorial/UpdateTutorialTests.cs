using TestCommon.Categories;
using TestCommon.Tutorials;

namespace CleanArchitecture.Application.SubcutaneousTests.Tutorials.Commands.UpdateTutorial;

[Collection(WebAppFactoryCollection.CollectionName)]
public class UpdateTutorialTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task UpdateTutorial_WhenTutorialExists_ShouldUpdateTutorial()
    {
        // Arrange
        var createResult = await _mediator.Send(TutorialCommandFactory.CreateCreateTutorialCommand());

        var command = TutorialCommandFactory.CreateUpdateTutorialCommand(
            tutorialId: createResult.Value.Id,
            title: "Updated title",
            published: true);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Title.Should().Be("Updated title");
        result.Value.Published.Should().BeTrue();
        result.Value.UpdatedAt.Should().BeOnOrAfter(result.Value.CreatedAt);
    }

    [Fact]
    public async Task UpdateTutorial_WhenTutorialDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var command = TutorialCommandFactory.CreateUpdateTutorialCommand(tutorialId: Guid.NewGuid());

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Description.Should().Contain("Tutorial");
    }

    [Fact]
    public async Task UpdateTutorial_WhenCategoryExists_ShouldAssignTheCategory()
    {
        // Arrange
        var createResult = await _mediator.Send(TutorialCommandFactory.CreateCreateTutorialCommand());
        var category = (await _mediator.Send(CategoryCommandFactory.CreateCreateCategoryCommand())).Value;

        var command = TutorialCommandFactory.CreateUpdateTutorialCommand(
            tutorialId: createResult.Value.Id,
            categoryId: category.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.CategoryId.Should().Be(category.Id);
    }

    [Fact]
    public async Task UpdateTutorial_WhenCategoryDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var createResult = await _mediator.Send(TutorialCommandFactory.CreateCreateTutorialCommand());

        var command = TutorialCommandFactory.CreateUpdateTutorialCommand(
            tutorialId: createResult.Value.Id,
            categoryId: Guid.NewGuid());

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Description.Should().Contain("Category");
    }
}
