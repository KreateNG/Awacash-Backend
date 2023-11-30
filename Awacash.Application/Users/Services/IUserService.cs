using Awacash.Application.Customers.DTOs;
using Awacash.Application.Users.DTOs;
using Awacash.Application.Users.FilterModels;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Users.Services
{
    public interface IUserService
    {
        Task<ResponseModel<UserDTO>> CreateUserAsync(string firstName, string lastName, string email, string phoneNumber, string roleId);
        Task<ResponseModel<UserDTO>> UpdateUserAsync(string Id, string firstName, string lastName, string email, string phoneNumber, string userName, string roleId);
        Task<ResponseModel<List<UserDTO>>> GetAllUserAsync();
        Task<ResponseModel<PagedResult<UserDTO>>> GetPaginatedUserAsync(UserFilterModel userFilterModel);
        Task<ResponseModel<UserDTO>> GetUserByIdAsync(string Id);
        Task<ResponseModel<UserDTO>> DeleteUserByAsync(string Id);
        Task<ResponseModel<string>> ChangePasswordAsync(string currentPassword, string newPassword);
    }
}
