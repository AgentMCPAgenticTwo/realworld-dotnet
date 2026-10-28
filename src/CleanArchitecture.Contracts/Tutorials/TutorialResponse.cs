namespace CleanArchitecture.Contracts.Tutorials;

public record TutorialResponse(
    Guid Id,
    string Title,
    string Description,
    bool Published,
    Guid? CategoryId,
    DateTime CreatedAt,
    DateTime UpdatedAt);
