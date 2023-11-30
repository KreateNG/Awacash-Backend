using AutoMapper;
using Awacash.Application.Authentication.Handler.Commands.ChangePassword;
using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Application.Customers.DTOs;
using Awacash.Application.Customers.Services;
using Awacash.Application.Customers.Specifications;
using Awacash.Application.Users.DTOs;
using Awacash.Application.Users.FilterModels;
using Awacash.Application.Users.Specifications;
using Awacash.Domain.Enums;
using Awacash.Domain.IdentityModel;
using Awacash.Domain.Interfaces;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
        private readonly ICryptoService _cryptoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IAwacashThirdPartyService _BerachahThirdPartyService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger, ICryptoService cryptoService, UserManager<ApplicationUser> userManager, IMapper mapper, ICurrentUser currentUser, IAwacashThirdPartyService berachahThirdPartyService, RoleManager<ApplicationRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cryptoService = cryptoService;
            _userManager = userManager;
            _mapper = mapper;
            _currentUser = currentUser;
            _BerachahThirdPartyService = berachahThirdPartyService;
            _roleManager = roleManager;
        }

        public async Task<ResponseModel<UserDTO>> CreateUserAsync(string firstName, string lastName, string email, string phoneNumber, string roleId)
        {
            try
            {
                var userWithEmail = await _unitOfWork.ApplicationUserRepository.GetByAsync(x => x.Email == email && x.IsActive != false);
                if (userWithEmail != null)
                {
                    return ResponseModel<UserDTO>.Failure($"User with email: {email} already exist");
                }

                var userWithPhoneMumber = await _unitOfWork.ApplicationUserRepository.GetByAsync(x => x.PhoneNumber == phoneNumber && x.IsActive != false);
                if (userWithPhoneMumber != null)
                {
                    return ResponseModel<UserDTO>.Failure($"User with phone number: {phoneNumber} already exist");
                }

                var role = await _roleManager.FindByIdAsync(roleId);
                if (role == null)
                    return ResponseModel<UserDTO>.Failure($"Invalid role ID '{roleId}'");

                //if (role.Status != Status.Active)
                //    throw new Exception($"Specified role '{theRole.Name}' is not active");


                var password = $"{lastName.ToUpper()}@123";
                var username = $"{lastName}@123";

                var user = new ApplicationUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    UserName = username,
                    IsActive = true
                };
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    _logger.LogError(result.Errors.FirstOrDefault()?.Description);
                    return ResponseModel<UserDTO>.Failure($"User creation failed. Reason ==> {result.Errors.FirstOrDefault()?.Description}");
                }

                await _userManager.AddToRoleAsync(user, role.Name);
                var userDto = new UserDTO()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    IsActive = user.IsActive,
                    Id = user.Id,
                }; //_mapper.Map<UserDTO>(user);

                var msge = $"<p>Dear {user.LastName} </p> <p>Your login crefential: Email: {user.Email}  and your Password: {password}</p>";

                _BerachahThirdPartyService.SendEmail(user.Email, "Sign up", msge);
                return ResponseModel<UserDTO>.Success(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while creating user: {ex.Message}", nameof(CreateUserAsync));
                return ResponseModel<UserDTO>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<UserDTO>> DeleteUserByAsync(string Id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Id);
                if (user == null)
                {
                    return ResponseModel<UserDTO>.Failure("User not found");
                }
                user.IsActive = false;
                await _userManager.UpdateAsync(user);
                var userDto = new UserDTO()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    IsActive = user.IsActive,
                    Id = user.Id,
                };
                return ResponseModel<UserDTO>.Success(userDto);
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while deletting user: {ex.Message}", nameof(DeleteUserByAsync));
                return ResponseModel<UserDTO>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<List<UserDTO>>> GetAllUserAsync()
        {
            try
            {
                var user = await _unitOfWork.ApplicationUserRepository.ListAllAsync();
                var userDto = user.Select(x => new UserDTO()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    UserName = x.UserName,
                    PhoneNumber = x.PhoneNumber,
                    IsActive = x.IsActive,
                    Id = x.Id,
                }).ToList(); //_mapper.Map<List<UserDTO>>(user.ToList());
                return ResponseModel<List<UserDTO>>.Success(userDto);
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while fetching users: {ex.Message}", nameof(GetAllUserAsync));
                return ResponseModel<List<UserDTO>>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<PagedResult<UserDTO>>> GetPaginatedUserAsync(UserFilterModel filterModel)
        {
            try
            {
                var userSpecification = new UserFilterSpecification(firstname: filterModel.FirstName, lastname: filterModel.LastName, id: filterModel.Id, email: filterModel.Email, phonenumber: filterModel.PhoneNumber, isActive: filterModel.IsActive);
                var users = await _unitOfWork.ApplicationUserRepository.ListAsync(filterModel.PageIndex, filterModel.PageSize, userSpecification);
                var response = new PagedResult<UserDTO>()
                {
                    Results = users.Results.Select(x => new UserDTO()
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                        UserName = x.UserName,
                        PhoneNumber = x.PhoneNumber,
                        IsActive = x.IsActive,
                        Id = x.Id,
                    }).ToList(),
                    CurrentPage = users.CurrentPage,
                    PageCount = users.PageCount,
                    RowCount = users.RowCount,
                    PageSize = users.PageSize
                };
                return ResponseModel<PagedResult<UserDTO>>.Success(response);

            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while fetching users: {ex.Message}", nameof(GetAllUserAsync));
                return ResponseModel<PagedResult<UserDTO>>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<UserDTO>> GetUserByIdAsync(string Id)
        {
            try
            {
                var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(Id);
                var userDto = new UserDTO()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    IsActive = user.IsActive,
                    Id = user.Id,
                };
                return ResponseModel<UserDTO>.Success(userDto);
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while getting user: {ex.Message}", nameof(GetUserByIdAsync));
                return ResponseModel<UserDTO>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<UserDTO>> UpdateUserAsync(string Id, string firstName, string lastName, string email, string phoneNumber, string username, string roleId)
        {
            try
            {
                var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(Id);
                if (user == null)
                {
                    return ResponseModel<UserDTO>.Failure("User not found");
                }
                if (!user.Email.Equals(email))
                {
                    var userWithEmail = await _unitOfWork.ApplicationUserRepository.GetByAsync(x => x.Email == email && x.IsActive != false);
                    if (userWithEmail != null)
                    {
                        return ResponseModel<UserDTO>.Failure($"User with email: {email} already exist");
                    }
                }
                if (!user.UserName.Equals(username))
                {
                    var userWithUsername = await _unitOfWork.ApplicationUserRepository.GetByAsync(x => x.UserName == username && x.IsActive != false);
                    if (userWithUsername != null)
                    {
                        return ResponseModel<UserDTO>.Failure($"User with username: {username} already exist");
                    }

                }

                if (!user.PhoneNumber.Equals(phoneNumber))
                {
                    var userWithPhoneMumber = await _unitOfWork.ApplicationUserRepository.GetByAsync(x => x.PhoneNumber == phoneNumber && x.IsActive != false);
                    if (userWithPhoneMumber != null)
                    {
                        return ResponseModel<UserDTO>.Failure($"User with phone number: {phoneNumber} already exist");
                    }
                }

                var role = await _roleManager.FindByIdAsync(roleId);
                if (role == null)
                    return ResponseModel<UserDTO>.Failure($"Invalid role ID '{roleId}'");

                user.PhoneNumber = phoneNumber;
                user.UserName = username;
                user.Email = email;
                user.FirstName = firstName;
                user.LastName = lastName;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    var error = result.Errors.Select(x => x.Description).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        return ResponseModel<UserDTO>.Failure(error);
                    }
                    return ResponseModel<UserDTO>.Failure($"fail to update user");
                }

                var existingRoles = await _userManager.GetRolesAsync(user);

                var rolesToRemove = existingRoles.Where(x => x != role.Name).ToList();

                foreach (var r in rolesToRemove)
                    await _userManager.RemoveFromRoleAsync(user, r);

                await _userManager.AddToRoleAsync(user, role.Name);

                var userDto = new UserDTO()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    IsActive = user.IsActive,
                    Id = user.Id,
                };
                return ResponseModel<UserDTO>.Success(userDto);
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while updating user: {ex.Message}", nameof(UpdateUserAsync));
                return ResponseModel<UserDTO>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<string>> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            try
            {
                var userId = _currentUser.GetUserId();
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return ResponseModel<string>.Failure("User not found");
                }


                var identityResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

                if (!identityResult.Succeeded)
                {
                    var response = new ResponseModel<string>();
                    response.Errors = identityResult.Errors.Select(x => x.Description).ToList();
                    response.Message = "An error occured!";
                    response.IsSuccessful = false;
                    return response;
                }
                return ResponseModel<string>.Success("Password changed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while changing user password: {ex.Message}", nameof(ChangePasswordAsync));
                return ResponseModel<string>.Failure("An exception occured");
            }
        }
    }
}
