

using API.DTOs;
using FluentValidation;

namespace API.Validators
{

    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {

        public UserLoginDtoValidator()
        {

            RuleFor(x => x.UsernameOrEmail).NotEmpty().WithMessage("Username or email is required.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
}