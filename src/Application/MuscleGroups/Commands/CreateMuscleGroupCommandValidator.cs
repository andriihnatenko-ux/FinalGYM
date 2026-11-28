using FluentValidation;

namespace Application.MuscleGroups.Commands;

public class CreateMuscleGroupCommandValidator : AbstractValidator<CreateMuscleGroupCommand>
{
    public CreateMuscleGroupCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255);
    }
}
