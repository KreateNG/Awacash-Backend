using Awacash.Application.Authentication.Common.Extentions;
using Awacash.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Infrastructure.Authentication.AuthGuard
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermissionAuthorizationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            string? userId = context.User?.GetUserId();
            if (userId != null)
            {
                var userRole = await _unitOfWork.ApplicationUserRoleRepository.GetByIdAsync(userId);
                if (userRole != null)
                {
                    var roleClaims = await _unitOfWork.ApplicationRoleClaimRepository.ListAsync(x => x.RoleId == userRole.RoleId);
                    if (roleClaims != null)
                    {
                        var exist = roleClaims.ToList().Any(x => x.ClaimValue == requirement.Permission);
                        if (exist)
                        {
                            context.Succeed(requirement);
                        }
                    }
                }
            }

        }
    }
}
