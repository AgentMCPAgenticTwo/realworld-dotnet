namespace CleanArchitecture.Contracts.Tutorials;

public record CreateTutorialRequest(
    string Title,
    string Description,
    bool Published);
