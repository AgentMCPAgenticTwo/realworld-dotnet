using FluentValidation;

namespace CleanArchitecture.Application.Tutorials.Commands.UpdateTutorial;

public class UpdateTutorialCommandValidator : AbstractValidator<UpdateTutorialCommand>
{
    public UpdateTutorialCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotNull().MaximumLength(10000);
    }
}
