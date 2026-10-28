using CleanArchitecture.Domain.Tutorials;

using TestCommon.TestConstants;

namespace TestCommon.Tutorials;

public static class TutorialFactory
{
    public static Tutorial CreateTutorial(
        string title = Constants.Tutorial.Title,
        string description = Constants.Tutorial.Description,
        bool published = false,
        DateTime? createdAt = null,
        Guid? id = null)
    {
        return new Tutorial(
            title,
            description,
            published,
            createdAt ?? DateTime.UtcNow,
            id ?? Constants.Tutorial.Id);
    }
}
