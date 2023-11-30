using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Role.Handler.Commands.CreateRole
{
    public class CreateRoleCommandValidation: AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("Role name is required");
            RuleFor(x => x.Description).NotEmpty().NotNull().WithMessage("Role description is required");
        }
    }
}
