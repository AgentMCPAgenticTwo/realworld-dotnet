using CleanArchitecture.Api.IntegrationTests.Common.WebApplicationFactory;
using CleanArchitecture.Contracts.Common;

namespace CleanArchitecture.Api.IntegrationTests.Controllers;

[Collection(WebAppFactoryCollection.CollectionName)]
public class HealthControllerTests
{
    private readonly HttpClient _client;

    public HealthControllerTests(WebAppFactory webAppFactory)
    {
        _client = webAppFactory.CreateClient();
    }

    [Fact]
    public async Task GetHealth_WhenAnonymous_ShouldReturnOkStatus()
    {
        // Act
        var response = await _client.GetAsync("api/health");

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.OK);

        var health = await response.Content.ReadFromJsonAsync<HealthResponse>();
        health.Should().NotBeNull();
        health!.Status.Should().Be("ok");
    }
}
