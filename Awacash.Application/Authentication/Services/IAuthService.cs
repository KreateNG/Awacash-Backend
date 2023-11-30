using Awacash.Application.Authentication.Common;
using Awacash.Shared;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Authentication.Services
{
    public interface IAuthService
    {
        Task<ResponseModel<AuthenticationResult>> GetTokenAsync(string email, string password, string deviceId, string ipAddress, CancellationToken cancellationToken);
        Task<ResponseModel<AdminAuthResult>> GetAdminTokenAsync(string email, string password, string ipAddress, CancellationToken cancellationToken);
        Task<ResponseModel<AuthenticationResult>> RegisterCustomer(string email, string password, string pin, string firstName, string lastName, string middleName, string phoneNumber, string hash, string referralCode, string ipAddress, CancellationToken cancellationToken);
        Task<ResponseModel<string>> SendForgotPasswordVerificationCode(string phoneNumber, string ipAddress, CancellationToken cancellationToken);
        Task<ResponseModel<string>> VerifyForgotPasswordCode(string code, string hash);
        Task<ResponseModel<bool>> ResetPassword(string email, string ConfirmPassword, string password, string hash);

        Task<ResponseModel<string>> SendUserForgotPasswordVerificationCode(string email, string ipAddress, CancellationToken cancellationToken);
        Task<ResponseModel<bool>> ResetUserPassword(string email, string confirmPassword, string password);

        Task<ResponseModel<AuthenticationResult>> RegisterCustomerWithAccount(string email, string password, string pin, string firstName, string lastName, string middleName, string phoneNumber, string accountId, string hash, string ipAddress, CancellationToken cancellationToken);
        Task<ResponseModel<string>> ValidateAccountNumber(string code, string hash);
        Task<ResponseModel<AccountValidationResponse>> SendAccountNumberVerificationCode(string accountNumber);
        Task<ResponseModel<AuthenticationResult>> RefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);


    }
}