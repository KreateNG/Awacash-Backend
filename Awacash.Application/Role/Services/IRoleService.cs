using Awacash.Application.Role.DTOs;
using Awacash.Application.Role.FilterModels;
using Awacash.Domain.Common.Models;
using Awacash.Domain.Enums;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Role.Services
{
    public interface IRoleService
    {
        Task<ResponseModel<bool>> CreateRoleAsync(string name, string description, List<Pemission> permmissions);
        Task<ResponseModel<bool>> UpdateRoleAsync(string roleId, string name, string description, List<int> permmissions);
        Task<ResponseModel<RoleDTO>> GetRoleByIdAsync(string roleId);
        Task<ResponseModel<PagedResult<RoleDTO>>> GetPaginatedRoleAsync(RoleFilterModel filterModel);
        Task<ResponseModel<List<RoleDTO>>> GetAllRoleAsync();
        Task<ResponseModel<List<PermissionDto>>> GetAllPermissionsAsync();
        Task<ResponseModel<bool>> DeleteRoleAsync(string roleId);

    }
}
