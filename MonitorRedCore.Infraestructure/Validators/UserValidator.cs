using FluentValidation;
using MonitorRedCore.Core.DTOs;

namespace MonitorRedCore.Infraestructure.Validators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(user => user.Email)
                .NotNull()
                .EmailAddress();

            RuleFor(user => user.FirstName)
                .NotNull();

            RuleFor(user => user.Password)
                .NotNull()
                .MinimumLength(8);


            RuleFor(user => user.Role)
                .NotNull();
        }
    }
}
