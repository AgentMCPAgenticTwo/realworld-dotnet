namespace CleanArchitecture.Contracts.Tutorials;

public record UpdateTutorialRequest(
    string Title,
    string Description,
    bool Published,
    Guid? CategoryId = null);
