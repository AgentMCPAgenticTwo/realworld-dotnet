using CleanArchitecture.Application.Tutorials.Commands.CreateTutorial;
using CleanArchitecture.Application.Tutorials.Commands.DeleteAllTutorials;
using CleanArchitecture.Application.Tutorials.Commands.DeleteTutorial;
using CleanArchitecture.Application.Tutorials.Commands.UpdateTutorial;

using TestCommon.TestConstants;

namespace TestCommon.Tutorials;

public static class TutorialCommandFactory
{
    public static CreateTutorialCommand CreateCreateTutorialCommand(
        string title = Constants.Tutorial.Title,
        string description = Constants.Tutorial.Description,
        bool published = false)
    {
        return new CreateTutorialCommand(title, description, published);
    }

    public static UpdateTutorialCommand CreateUpdateTutorialCommand(
        Guid? tutorialId = null,
        string title = Constants.Tutorial.Title,
        string description = Constants.Tutorial.Description,
        bool published = true)
    {
        return new UpdateTutorialCommand(
            tutorialId ?? Constants.Tutorial.Id,
            title,
            description,
            published);
    }

    public static DeleteTutorialCommand CreateDeleteTutorialCommand(Guid? tutorialId = null)
    {
        return new DeleteTutorialCommand(tutorialId ?? Constants.Tutorial.Id);
    }

    public static DeleteAllTutorialsCommand CreateDeleteAllTutorialsCommand()
    {
        return new DeleteAllTutorialsCommand();
    }
}
