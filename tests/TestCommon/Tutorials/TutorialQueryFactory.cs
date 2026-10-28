using CleanArchitecture.Application.Tutorials.Queries.GetTutorial;
using CleanArchitecture.Application.Tutorials.Queries.ListTutorials;

using TestCommon.TestConstants;

namespace TestCommon.Tutorials;

public static class TutorialQueryFactory
{
    public static GetTutorialQuery CreateGetTutorialQuery(Guid? tutorialId = null)
    {
        return new GetTutorialQuery(tutorialId ?? Constants.Tutorial.Id);
    }

    public static ListTutorialsQuery CreateListTutorialsQuery(string? title = null)
    {
        return new ListTutorialsQuery(title);
    }
}
