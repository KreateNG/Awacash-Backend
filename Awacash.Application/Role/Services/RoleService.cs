using AutoMapper;
using Awacash.Application.Role.DTOs;
using Awacash.Application.Role.FilterModels;
using Awacash.Application.Role.Specifications;
using Awacash.Domain.Common.Constants;
using Awacash.Domain.Common.Models;
using Awacash.Domain.Enums;
using Awacash.Domain.Helpers;
using Awacash.Domain.IdentityModel;
using Awacash.Domain.Interfaces;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Awacash.Application.Role.Services
{
    public class RoleService : IRoleService
    {
        private readonly ILogger<RoleService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public object CustomClaimTypes { get; private set; }

        public RoleService(ILogger<RoleService> logger, IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseModel<bool>> CreateRoleAsync(string name, string description, List<Pemission> permissions)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(name);
                if (role != null)
                {
                    return ResponseModel<bool>.Failure($"Role with name {name} already exist");
                }

                var newRole = new ApplicationRole
                {
                    Name = name,
                    Description = name
                };

                await _roleManager.CreateAsync(newRole);

                foreach (var permission in permissions)
                {

                    await _roleManager.AddClaimAsync(newRole, new Claim(ClaimsTypeConstant.Permission, Enum.GetName(typeof(Pemission), permission)));

                }

                await _unitOfWork.Complete();
                return ResponseModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while saving role: {ex.Message}", nameof(CreateRoleAsync));
                return ResponseModel<bool>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<PagedResult<RoleDTO>>> GetPaginatedRoleAsync(RoleFilterModel filterModel)
        {
            try
            {
                var rolespecification = new RoleFilterSpecification(name: filterModel.Name);
                var roles = await _unitOfWork.ApplicationRoleRepository.ListAsync(filterModel.PageIndex, filterModel.PageSize, rolespecification);
                return ResponseModel<PagedResult<RoleDTO>>.Success(_mapper.Map<PagedResult<RoleDTO>>(roles));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting roles: {ex.Message}", nameof(GetAllRoleAsync));
                return ResponseModel<PagedResult<RoleDTO>>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<List<RoleDTO>>> GetAllRoleAsync()
        {
            try
            {

                var roles = await _unitOfWork.ApplicationRoleRepository.ListAllAsync();
                return ResponseModel<List<RoleDTO>>.Success(_mapper.Map<List<RoleDTO>>(roles));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting roles: {ex.Message}", nameof(GetAllRoleAsync));
                return ResponseModel<List<RoleDTO>>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<RoleDTO>> GetRoleByIdAsync(string roleId)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role is null)
                {
                    return ResponseModel<RoleDTO>.Failure("Specified role identifier was not found");
                }
                var roleDto = _mapper.Map<RoleDTO>(role);
                roleDto.Pemissions = GetRolePermissions(role);
                return ResponseModel<RoleDTO>.Success(roleDto);
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while getting role: {ex.Message}", nameof(GetRoleByIdAsync));
                return ResponseModel<RoleDTO>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<bool>> UpdateRoleAsync(string roleId, string name, string description, List<int> permmissions)
        {
            try
            {
                var role = await _unitOfWork.ApplicationRoleRepository.GetByIdAsync(roleId);
                if (role is null)
                {
                    return ResponseModel<bool>.Failure("Specified role identifier was not found");
                }

                if (role.Name.ToLower().Equals(name.ToLower()))
                {
                    return ResponseModel<bool>.Failure($"Role with name {name} already exist");
                }
                var rolePermmissions = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in rolePermmissions)
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }

                role.Name = name;
                role.Description = description;
                await _roleManager.UpdateAsync(role);


                foreach (var permission in permmissions)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(ClaimsTypeConstant.Permission, Enum.GetName(typeof(Pemission), permission)));
                }

                await _unitOfWork.Complete();
                return ResponseModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while updating role: {ex.Message}", nameof(UpdateRoleAsync));
                return ResponseModel<bool>.Failure("Exception error");
            }
        }
        public async Task<ResponseModel<bool>> DeleteRoleAsync(string roleId)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role == null)
                {
                    return ResponseModel<bool>.Failure($"Role not found");
                }

                await _roleManager.DeleteAsync(role);

                return ResponseModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while deleting role: {ex.Message}", nameof(DeleteRoleAsync));
                return ResponseModel<bool>.Failure("Exception error");
            }
        }


        public Task<ResponseModel<List<PermissionDto>>> GetAllPermissionsAsync()
        {
            List<PermissionDto> permissions = null;
            try
            {
                permissions = PermissionHelper.GetPermissionsToDisplay(typeof(Pemission));
                return Task.FromResult(ResponseModel<List<PermissionDto>>.Success(permissions));
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while getting permissions: {ex.Message}", nameof(GetRoleByIdAsync));
                return Task.FromResult(ResponseModel<List<PermissionDto>>.Failure("Exception error"));
            }
        }

        private List<PermissionDto> GetRolePermissions(ApplicationRole role)
        {
            List<PermissionDto> result = new List<PermissionDto>();
            try
            {
                var roleClaims = _roleManager.GetClaimsAsync(role).Result;
                var permissions = roleClaims.Where(x => x.Type == ClaimsTypeConstant.Permission)
                                                  .Select(x => x.Value).ToList();
                if (permissions != null && permissions.Count > 0)
                {
                    //var perm = permissions.FirstOrDefault().UnpackPermissionsFromString().ToList();
                    var list = PermissionHelper.GetPermissionsToDisplay(typeof(Pemission));
                    result = list.Where(x => permissions.Any(p => p == Enum.GetName(typeof(Pemission), x.Permission))).ToList();
                }
            }
            catch { }

            return result;
        }


    }
}
