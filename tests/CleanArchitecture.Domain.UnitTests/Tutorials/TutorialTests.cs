using TestCommon.Tutorials;

namespace CleanArchitecture.Domain.UnitTests.Tutorials;

public class TutorialTests
{
    [Fact]
    public void CreateTutorial_WhenConstructedSuccessfully_ShouldHaveMatchingCreatedAndUpdatedDates()
    {
        // Act
        var tutorial = TutorialFactory.CreateTutorial();

        // Assert
        tutorial.Published.Should().BeFalse();
        tutorial.UpdatedAt.Should().Be(tutorial.CreatedAt);
    }

    [Fact]
    public void UpdateTutorial_WhenUpdated_ShouldOverwriteValuesAndBumpUpdatedAt()
    {
        // Arrange
        var createdAt = DateTime.UtcNow;
        var tutorial = TutorialFactory.CreateTutorial(createdAt: createdAt);
        var updatedAt = createdAt.AddMinutes(5);

        // Act
        tutorial.Update("New title", "New description", published: true, updatedAt);

        // Assert
        tutorial.Title.Should().Be("New title");
        tutorial.Description.Should().Be("New description");
        tutorial.Published.Should().BeTrue();
        tutorial.CreatedAt.Should().Be(createdAt);
        tutorial.UpdatedAt.Should().Be(updatedAt);
    }
}
