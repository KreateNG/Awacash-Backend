using System;
using Awacash.Domain.Common.Models;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Role.Handler.Queries.GetPermissions
{
	public record GetPermissionsQuery():IRequest<ResponseModel<List<PermissionDto>>>;
}

