using TestCommon.Tutorials;

namespace CleanArchitecture.Application.SubcutaneousTests.Tutorials.Queries.ListTutorials;

[Collection(WebAppFactoryCollection.CollectionName)]
public class ListTutorialsTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task ListTutorials_WhenNoTitleFilter_ShouldReturnAllTutorials()
    {
        // Arrange
        await _mediator.Send(TutorialCommandFactory.CreateCreateTutorialCommand(title: "Angular basics"));
        await _mediator.Send(TutorialCommandFactory.CreateCreateTutorialCommand(title: "Dotnet basics"));

        // Act
        var result = await _mediator.Send(TutorialQueryFactory.CreateListTutorialsQuery());

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task ListTutorials_WhenTitleFilterProvided_ShouldReturnMatchingTutorials()
    {
        // Arrange
        await _mediator.Send(TutorialCommandFactory.CreateCreateTutorialCommand(title: "Angular basics"));
        await _mediator.Send(TutorialCommandFactory.CreateCreateTutorialCommand(title: "Dotnet basics"));

        // Act
        var result = await _mediator.Send(TutorialQueryFactory.CreateListTutorialsQuery("Angular"));

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().ContainSingle();
        result.Value[0].Title.Should().Be("Angular basics");
    }
}
