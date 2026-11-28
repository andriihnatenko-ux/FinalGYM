using FluentValidation;

namespace Application.Exercises.Commands;

public class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
{
    public CreateExerciseCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(500);

        RuleFor(x => x.CaloriesPerHour)
            .InclusiveBetween(1, 5_000);

        RuleFor(x => x.Sets)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.Reps)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.MuscleGroupIds)
            .NotEmpty();

        RuleForEach(x => x.MuscleGroupIds)
            .NotEqual(Guid.Empty);
    }
}
