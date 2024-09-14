using FluentValidation;
using Poseidon.DTOs;

namespace Poseidon.Validators
{
    public class PassengerValidator : AbstractValidator<PassengerDTO>
    {
        public PassengerValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(p => p.Age).InclusiveBetween(0, 120).When(p => p.Age.HasValue).WithMessage("Age must be between 0 and 120.");
            RuleFor(p => p.Sex).NotEmpty().WithMessage("Sex is required.");
            RuleFor(p => p.Fare).GreaterThan(0).WithMessage("Fare must be a positive value.");
        }
    }
}
