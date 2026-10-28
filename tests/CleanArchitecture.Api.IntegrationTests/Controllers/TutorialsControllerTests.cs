using CleanArchitecture.Api.IntegrationTests.Common.WebApplicationFactory;
using CleanArchitecture.Contracts.Tutorials;

namespace CleanArchitecture.Api.IntegrationTests.Controllers;

[Collection(WebAppFactoryCollection.CollectionName)]
public class TutorialsControllerTests
{
    private readonly HttpClient _client;

    public TutorialsControllerTests(WebAppFactory webAppFactory)
    {
        _client = webAppFactory.CreateClient();
        webAppFactory.ResetDatabase();
    }

    [Fact]
    public async Task CreateTutorial_WhenAnonymous_ShouldCreateAndBeRetrievable()
    {
        // Arrange
        var request = new CreateTutorialRequest("Angular basics", "An angular tutorial", Published: false);

        // Act
        var createResponse = await _client.PostAsJsonAsync("api/tutorials", request);

        // Assert
        createResponse.Should().HaveStatusCode(HttpStatusCode.Created);

        var created = await createResponse.Content.ReadFromJsonAsync<TutorialResponse>();
        created.Should().NotBeNull();
        created!.Title.Should().Be(request.Title);
        created.Published.Should().BeFalse();
        created.CreatedAt.Should().Be(created.UpdatedAt);

        var getResponse = await _client.GetAsync($"api/tutorials/{created.Id}");
        getResponse.Should().HaveStatusCode(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateTutorial_WhenPublishedIsOmitted_ShouldDefaultToFalse()
    {
        // Act
        var createResponse = await _client.PostAsJsonAsync(
            "api/tutorials",
            new { title = "Angular basics", description = "An angular tutorial" });

        // Assert
        createResponse.Should().HaveStatusCode(HttpStatusCode.Created);

        var created = (await createResponse.Content.ReadFromJsonAsync<TutorialResponse>())!;
        created.Published.Should().BeFalse();
        created.CategoryId.Should().BeNull();
    }

    [Fact]
    public async Task GetTutorial_WhenIdDoesNotExist_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync($"api/tutorials/{Guid.NewGuid()}");

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateTutorial_WhenTutorialExists_ShouldReturnMessage()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync(
            "api/tutorials",
            new CreateTutorialRequest("Angular basics", "An angular tutorial", Published: false));

        var created = (await createResponse.Content.ReadFromJsonAsync<TutorialResponse>())!;

        // Act
        var updateResponse = await _client.PutAsJsonAsync(
            $"api/tutorials/{created.Id}",
            new UpdateTutorialRequest(created.Title, created.Description, Published: true));

        // Assert
        updateResponse.Should().HaveStatusCode(HttpStatusCode.OK);

        var message = await updateResponse.Content.ReadFromJsonAsync<MessageResponse>();
        message!.Message.Should().NotBeNullOrEmpty();

        var updated = await _client.GetFromJsonAsync<TutorialResponse>($"api/tutorials/{created.Id}");
        updated!.Published.Should().BeTrue();
    }

    [Fact]
    public async Task ListTutorials_WhenTitleQueryProvided_ShouldReturnMatchingTutorialsOnly()
    {
        // Arrange
        await _client.PostAsJsonAsync(
            "api/tutorials",
            new CreateTutorialRequest("Angular basics", "An angular tutorial", Published: false));

        await _client.PostAsJsonAsync(
            "api/tutorials",
            new CreateTutorialRequest("Dotnet basics", "A dotnet tutorial", Published: false));

        // Act
        var tutorials = await _client.GetFromJsonAsync<List<TutorialResponse>>("api/tutorials?title=Angular");

        // Assert
        tutorials.Should().ContainSingle();
        tutorials![0].Title.Should().Be("Angular basics");
    }

    [Fact]
    public async Task DeleteTutorial_WhenTutorialExists_ShouldRemoveIt()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync(
            "api/tutorials",
            new CreateTutorialRequest("Angular basics", "An angular tutorial", Published: false));

        var created = (await createResponse.Content.ReadFromJsonAsync<TutorialResponse>())!;

        // Act
        var deleteResponse = await _client.DeleteAsync($"api/tutorials/{created.Id}");

        // Assert
        deleteResponse.Should().HaveStatusCode(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"api/tutorials/{created.Id}");
        getResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteAllTutorials_WhenTutorialsExist_ShouldRemoveThemAll()
    {
        // Arrange
        await _client.PostAsJsonAsync(
            "api/tutorials",
            new CreateTutorialRequest("Angular basics", "An angular tutorial", Published: false));

        // Act
        var deleteResponse = await _client.DeleteAsync("api/tutorials");

        // Assert
        deleteResponse.Should().HaveStatusCode(HttpStatusCode.OK);

        var tutorials = await _client.GetFromJsonAsync<List<TutorialResponse>>("api/tutorials");
        tutorials.Should().BeEmpty();
    }
}
