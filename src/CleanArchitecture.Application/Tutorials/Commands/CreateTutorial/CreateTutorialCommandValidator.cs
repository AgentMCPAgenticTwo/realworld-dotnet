using FluentValidation;

namespace CleanArchitecture.Application.Tutorials.Commands.CreateTutorial;

public class CreateTutorialCommandValidator : AbstractValidator<CreateTutorialCommand>
{
    public CreateTutorialCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotNull().MaximumLength(10000);
    }
}
