using FluentValidation;
using MonitorRedCore.Core.DTOs;

namespace MonitorRedCore.Infraestructure.Validators
{
    public class RoleValidator : AbstractValidator<RoleDto>
    {
        public RoleValidator()
        {
            RuleFor(role => role.RoleType)
                .NotNull();
        }
    }
}
