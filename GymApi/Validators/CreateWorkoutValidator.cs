using FluentValidation;
using GymApi.Model;

namespace GymApi.Validators
{
    public class CreateWorkoutValidator : AbstractValidator<CreateWorkout>
    {
        public CreateWorkoutValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.Date)
                .NotNull().WithMessage("Date is required");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleForEach(x => x.Exercises)
                .ChildRules(exercise =>
                {
                    exercise.RuleFor(e => e.WorkoutExercise)
                        .NotEmpty().WithMessage("WorkoutExercise id is required");

                    exercise.RuleFor(e => e.SetAmount)
                        .NotEmpty().WithMessage("Exercise Amount is required")
                        .GreaterThan(0).WithMessage("SetAmount must be positive");

                    exercise.RuleFor(e => e.Description)
                        .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
                });
        }
    }
}
