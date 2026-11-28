using FluentValidation;

namespace Application.Exercises.Commands;

public class AddExerciseImageCommandValidator : AbstractValidator<AddExerciseImageCommand>
{
    public AddExerciseImageCommandValidator()
    {
        RuleFor(x => x.ExerciseId)
            .NotEmpty();

        RuleFor(x => x.OriginalName)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Content)
            .NotNull();
    }
}
