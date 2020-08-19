using FluentValidation;
using MonitorRedCore.Core.DTOs;

namespace MonitorRedCore.Infraestructure.Validators
{
    public class AuthValidator : AbstractValidator<AuthDto>
    {
        public AuthValidator()
        {
            RuleFor(user => user.Email)
                .NotNull()
                .EmailAddress();

            RuleFor(user => user.Password)
                .NotNull();
        }
    }
}
