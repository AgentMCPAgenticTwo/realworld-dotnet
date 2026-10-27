using TestCommon.Tutorials;

namespace CleanArchitecture.Application.SubcutaneousTests.Tutorials.Commands.DeleteTutorial;

[Collection(WebAppFactoryCollection.CollectionName)]
public class DeleteTutorialTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task DeleteTutorial_WhenTutorialExists_ShouldDeleteTutorial()
    {
        // Arrange
        var createResult = await _mediator.Send(TutorialCommandFactory.CreateCreateTutorialCommand());

        var command = TutorialCommandFactory.CreateDeleteTutorialCommand(createResult.Value.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeFalse();

        var getResult = await _mediator.Send(
            TutorialQueryFactory.CreateGetTutorialQuery(createResult.Value.Id));

        getResult.IsError.Should().BeTrue();
        getResult.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task DeleteTutorial_WhenTutorialDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var command = TutorialCommandFactory.CreateDeleteTutorialCommand(Guid.NewGuid());

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task DeleteAllTutorials_WhenTutorialsExist_ShouldDeleteThemAll()
    {
        // Arrange
        await _mediator.Send(TutorialCommandFactory.CreateCreateTutorialCommand(title: "First"));
        await _mediator.Send(TutorialCommandFactory.CreateCreateTutorialCommand(title: "Second"));

        // Act
        var result = await _mediator.Send(TutorialCommandFactory.CreateDeleteAllTutorialsCommand());

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(2);

        var listResult = await _mediator.Send(TutorialQueryFactory.CreateListTutorialsQuery());
        listResult.Value.Should().BeEmpty();
    }
}
