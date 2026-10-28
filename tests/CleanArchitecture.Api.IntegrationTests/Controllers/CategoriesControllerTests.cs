using CleanArchitecture.Api.IntegrationTests.Common.WebApplicationFactory;
using CleanArchitecture.Contracts.Categories;
using CleanArchitecture.Contracts.Tutorials;

namespace CleanArchitecture.Api.IntegrationTests.Controllers;

[Collection(WebAppFactoryCollection.CollectionName)]
public class CategoriesControllerTests
{
    private readonly HttpClient _client;

    public CategoriesControllerTests(WebAppFactory webAppFactory)
    {
        _client = webAppFactory.CreateClient();
        webAppFactory.ResetDatabase();
    }

    [Fact]
    public async Task CreateCategory_WhenAnonymous_ShouldCreateAndBeListed()
    {
        // Arrange
        var request = new CreateCategoryRequest("Architecture");

        // Act
        var createResponse = await _client.PostAsJsonAsync("api/categories", request);

        // Assert
        createResponse.Should().HaveStatusCode(HttpStatusCode.Created);

        var created = await createResponse.Content.ReadFromJsonAsync<CategoryResponse>();
        created.Should().NotBeNull();
        created!.Name.Should().Be(request.Name);
        created.Id.Should().NotBeEmpty();

        var categories = await _client.GetFromJsonAsync<List<CategoryResponse>>("api/categories");
        categories.Should().ContainSingle();
        categories![0].Id.Should().Be(created.Id);
    }

    [Fact]
    public async Task CreateCategory_WhenNameIsEmpty_ShouldReturnBadRequest()
    {
        // Act
        var response = await _client.PostAsJsonAsync("api/categories", new CreateCategoryRequest(string.Empty));

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTutorial_WhenCategoryExists_ShouldReturnTheCategoryId()
    {
        // Arrange
        var createCategoryResponse = await _client.PostAsJsonAsync(
            "api/categories",
            new CreateCategoryRequest("Architecture"));

        var category = (await createCategoryResponse.Content.ReadFromJsonAsync<CategoryResponse>())!;

        // Act
        var createResponse = await _client.PostAsJsonAsync(
            "api/tutorials",
            new CreateTutorialRequest("Angular basics", "An angular tutorial", Published: false, category.Id));

        // Assert
        createResponse.Should().HaveStatusCode(HttpStatusCode.Created);

        var created = (await createResponse.Content.ReadFromJsonAsync<TutorialResponse>())!;
        created.CategoryId.Should().Be(category.Id);

        var fetched = await _client.GetFromJsonAsync<TutorialResponse>($"api/tutorials/{created.Id}");
        fetched!.CategoryId.Should().Be(category.Id);
    }

    [Fact]
    public async Task CreateTutorial_WhenCategoryDoesNotExist_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            "api/tutorials",
            new CreateTutorialRequest("Angular basics", "An angular tutorial", Published: false, Guid.NewGuid()));

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateTutorial_WhenCategoryDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync(
            "api/tutorials",
            new CreateTutorialRequest("Angular basics", "An angular tutorial", Published: false));

        var created = (await createResponse.Content.ReadFromJsonAsync<TutorialResponse>())!;

        // Act
        var updateResponse = await _client.PutAsJsonAsync(
            $"api/tutorials/{created.Id}",
            new UpdateTutorialRequest(created.Title, created.Description, Published: true, Guid.NewGuid()));

        // Assert
        updateResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
