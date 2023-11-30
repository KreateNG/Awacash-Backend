using Awacash.Application.Common.Model;
using Awacash.Application.Customers.DTOs;
using Awacash.Application.Customers.FilterModels;
using Awacash.Domain.Models.Customer;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Services
{
    public interface ICustomerService
    {
        Task<ResponseModel<bool>> SetPin(string pin);
        Task<ResponseModel<bool>> ChangePin(string oldPin, string newPin);
        Task<ResponseModel<bool>> ChangePassword(string oldPassword, string newPassword);
        Task<ResponseModel<string>> VerificationPhoneNumber(string code, string hash);
        Task<ResponseModel<string>> SendPhoneNumberVerificationCode(string phoneNumber);

        Task<ResponseModel<List<CustomerDTO>>> GetAllCustomersAsync();
        Task<ResponseModel<PagedResult<CustomerDTO>>> GetPaginatedCustomerAsync(CustomerFilterModel filterModel);
        Task<ResponseModel<CustomerDTO>> GetCustomerByIdAsync(string customerId);
        Task<ResponseModel<CustomerAccountBalanceDTO>> GetCustomerBalanceAsync();
        Task<ResponseModel<string>> UploadCustomerProfileImageAsync(string PasswordBase64);
        Task<ResponseModel<string>> InitializeBvnAuthenticationAsync(string Bvn);
        Task<ResponseModel<BvnCustomerInfo>> GetBvnCustomerInfoWithAccessCode(string accessCode);

        Task<ResponseModel<bool>> ValidateCustomerBvn(string firstName, string lastName, DateTime dateOfBirth, string accessToken);
        Task<ResponseModel<bool>> RegisterkMobileDevice(string phone, string deviceId, string otp);
        Task<ResponseModel<bool>> UpdateCustomerAddressAndNextOfKin(string address, string state, string city, string nextOfKinName, string nextOfKinRelationship, string nextOfKinPhoneNumber, string nextOfKinCountry, string NextOfKinEmail, string NextOfKinAddress);

        Task<ResponseModel<List<AccountDTO>>> GetAllCustomerAccountsAsync();

        Task<ResponseModel> RequestStatement(string accountNumber, DateTime from, DateTime to);
        Task<ResponseModel<CustomerDTO>> UpdateProfile(string lastname, string firstname, string middlename);
        Task<BvnVerificationModel> TestCustomerBvn(string firstName, string lastName, DateTime dateOfBirth, string accessToken);
        Task<int> ProcessRefeerral();
        Task<ResponseModel<List<ReferralDTO>>> GetReferee();
    }
}