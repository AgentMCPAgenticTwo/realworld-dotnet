using TestCommon.Tutorials;

namespace CleanArchitecture.Application.SubcutaneousTests.Tutorials.Queries.GetTutorial;

[Collection(WebAppFactoryCollection.CollectionName)]
public class GetTutorialTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task GetTutorial_WhenTutorialExists_ShouldReturnTutorial()
    {
        // Arrange
        var createResult = await _mediator.Send(TutorialCommandFactory.CreateCreateTutorialCommand());

        var query = TutorialQueryFactory.CreateGetTutorialQuery(createResult.Value.Id);

        // Act
        var result = await _mediator.Send(query);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Id.Should().Be(createResult.Value.Id);
        result.Value.Title.Should().Be(createResult.Value.Title);
    }

    [Fact]
    public async Task GetTutorial_WhenTutorialDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var query = TutorialQueryFactory.CreateGetTutorialQuery(Guid.NewGuid());

        // Act
        var result = await _mediator.Send(query);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Description.Should().Contain("Tutorial");
    }
}
