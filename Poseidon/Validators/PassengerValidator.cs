using FluentValidation;
using Poseidon.DTOs;

namespace Poseidon.Validators
{
    public class CreatePassengerValidator : AbstractValidator<PassengerDTO>
    {
        public CreatePassengerValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Passenger name is required.")
                .MaximumLength(100).WithMessage("Passenger name must not exceed 100 characters.");

            RuleFor(p => p.Sex)
                .NotEmpty().WithMessage("Sex is required.")
                .Must(s => s == "male" || s == "female").WithMessage("Sex must be 'male' or 'female'.");

            RuleFor(p => p.Age)
                .GreaterThan(0).WithMessage("Age must be greater than 0.");

            RuleFor(p => p.Pclass)
                .InclusiveBetween(1, 3).WithMessage("Pclass must be 1, 2, or 3.");

            RuleFor(p => p.Fare)
                .GreaterThan(0).WithMessage("Fare must be greater than 0.");
        }
    }

    public class UpdatePassengerValidator : AbstractValidator<PassengerDTO>
    {
        public UpdatePassengerValidator()
        {
            RuleFor(p => p.Name)
                .MaximumLength(100).WithMessage("Passenger name must not exceed 100 characters.")
                .When(p => !string.IsNullOrEmpty(p.Name));

            RuleFor(p => p.Sex)
                .Must(s => s == "male" || s == "female").WithMessage("Sex must be 'male' or 'female'.")
                .When(p => !string.IsNullOrEmpty(p.Sex));

            RuleFor(p => p.Age)
                .GreaterThan(0).WithMessage("Age must be greater than 0.")
                .When(p => p.Age.HasValue);

            RuleFor(p => p.Pclass)
                .InclusiveBetween(1, 3).WithMessage("Pclass must be 1, 2, or 3.")
                .When(p => p.Pclass.HasValue);

            RuleFor(p => p.Fare)
                .GreaterThan(0).WithMessage("Fare must be greater than 0.")
                .When(p => p.Fare.HasValue);
        }
    }

}
