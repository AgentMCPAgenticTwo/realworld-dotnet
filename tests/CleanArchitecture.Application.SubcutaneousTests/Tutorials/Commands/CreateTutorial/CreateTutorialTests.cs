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
