using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(o => o.Username)
            .NotEmpty()
            .WithMessage("{Username} is required.")
            .NotNull()
            .MaximumLength(70)
            .WithMessage("{Username} must not exceed 70 characters.");

        RuleFor(o => o.TotalPrice)
            .NotEmpty()
            .WithMessage("{TotalPrice0} is required.")
            .GreaterThan(-1)
            .WithMessage("{Total Price} should not be negative.");

        RuleFor(o => o.EmailAddess)
            .NotEmpty()
            .WithMessage("{EmailAddress} is required.");

        RuleFor(o => o.FirstName)
            .NotEmpty()
            .NotNull()
            .WithMessage("{FirstName} is required.");

        RuleFor(o => o.LastName)
            .NotEmpty()
            .NotNull()
            .WithMessage("{LastName} is required.");
    }
}
