using FluentValidation;
using API.DTOs;


namespace API.Validators
{

    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {

        public UserRegisterDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
            .WithMessage("First name is required.")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters.");

            RuleFor(x => x.Surname).NotEmpty()
            .WithMessage("Last name is required.")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters.");

            RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(13).WithMessage("You must be at least 13 years old to register.");

            RuleFor(x => x.Username).NotEmpty()
            .WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.");

            RuleFor(x => x.Email).NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please enter a valid email address. (example@domain.com)");

            RuleFor(x => x.Password).NotEmpty()
            .WithMessage("Password is required.")
            .Matches(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$")
            .WithMessage("Password must be at least 8 characters long and include an uppercase letter, a number, and a special character.");
        }
    }

}