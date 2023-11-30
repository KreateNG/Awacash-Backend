using Awacash.Application.Authentication.Common;
using Awacash.Domain.Interfaces;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Domain.Entities;
using AutoMapper;
using Awacash.Application.Common.Model;
using Awacash.Shared;
using Microsoft.Extensions.Logging;
using Awacash.Domain.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Awacash.Domain.Helpers;
using Awacash.Domain.Enums;
using Awacash.Domain.Extentions;
using AwaCash.Application.Common.Interfaces.Services;
using AwaCash.Application.Common.Exceptions;
using static AwaCash.Shared.Constants.TokenResourceConstants;
using Awacash.Application.Common.Exceptions;
using Awacash.Application.Authentication.Common.Extentions;
using System.Security.Cryptography;
using Awacash.Domain.Settings;
using Microsoft.Extensions.Options;

namespace Awacash.Application.Authentication.Services;

public class AuthService : IAuthService
{
    private ILogger<AuthService> _logger;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ICryptoService _cryptoService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAwacashThirdPartyService _BerachahThirdPartyService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOtpService _otpService;
    private readonly IBankOneAccountService _bankOneAccountService;
    private readonly AppSettings _appSettings;
    public AuthService(IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork, ICryptoService cryptoService, IDateTimeProvider dateTimeProvider, IMapper mapper, IAwacashThirdPartyService berachahThirdPartyService, ILogger<AuthService> logger, UserManager<ApplicationUser> userManager, IOtpService otpService, IBankOneAccountService bankOneAccountService, IOptions<AppSettings> appSettings)
    {

        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
        _cryptoService = cryptoService;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
        _BerachahThirdPartyService = berachahThirdPartyService;
        _logger = logger;
        _userManager = userManager;
        _otpService = otpService;
        _bankOneAccountService = bankOneAccountService;
        _appSettings = appSettings.Value;
    }
    public async Task<ResponseModel<AuthenticationResult>> GetTokenAsync(string email, string password, string deviceId, string ipAddress, CancellationToken cancellationToken)
    {


        var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Email == email && x.IsDeleted == false);
        if (customer is null)
        {
            return ResponseModel<AuthenticationResult>.Failure("Invalid cretentials");

        }
        if (_cryptoService.ComputeSaltedHash(customer.PasswordSalt, password) != customer.SaltedHashedPassword)
        {
            customer.PasswordTries++;
            _unitOfWork.CustomerRepository.Update(customer);
            await _unitOfWork.Complete();

            return ResponseModel<AuthenticationResult>.Failure("Invalid cretentials");
        }
        //if (!string.IsNullOrWhiteSpace(customer.DeviceId))
        //{
        //    if (customer.DeviceId != deviceId)
        //    {

        //        return ResponseModel<AuthenticationResult>.Success(new AuthenticationResult(null, null, true));
        //    }
        //}
        //else
        //{
        //    return ResponseModel<AuthenticationResult>.Success(new AuthenticationResult(null, null, true));
        //}
        var resfreshToken = GenerateRefreshToken();
        customer.RefreshToken = resfreshToken;
        customer.RefreshTokenExpiryTime = _dateTimeProvider.UtcNow.AddDays(6);

        _unitOfWork.CustomerRepository.Update(customer);

        var token = _jwtTokenGenerator.GenerateToken(customer.Id, customer.FirstName, customer.LastName);
        var customerDto = _mapper.Map<CustomerDTO>(customer);

        var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.Login);
        if (emailTemplate != null)
        {
            var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
            await _BerachahThirdPartyService.SendEmail(customer.Email, emailTemplate.Subject.Replace("[CUSTOMER_NAME]", customer.FullName), mssge);
        }

        return ResponseModel<AuthenticationResult>.Success(new AuthenticationResult(customerDto, token, resfreshToken));
    }

    public async Task<ResponseModel<AuthenticationResult>> RegisterCustomer(string email, string password, string pin, string firstName, string lastName, string middleName, string phoneNumber, string hash, string referralCode, string ipAddress, CancellationToken cancellationToken)
    {
        var hashId = _cryptoService.DecryptText(hash);

        var security = await _otpService.GetSingleAsync(hashId);
        if (!security.IsSuccessful || security.Data == null)
            throw new ResourceNotFoundException("Security code not found");

        if (!security.Data.IsUsed)
            return ResponseModel<AuthenticationResult>.Failure("Customer not validated");

        if (security.Data.ResourceId != phoneNumber)
            return ResponseModel<AuthenticationResult>.Failure("Customer not validated");


        var customerWithEmail = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Email == email && x.IsDeleted == false);
        if (customerWithEmail is not null)
        {
            return ResponseModel<AuthenticationResult>.Failure("Email already in use");

        }
        var customerWithPhoneNumber = await _unitOfWork.CustomerRepository.GetByAsync(x => x.PhoneNumber == phoneNumber && x.IsDeleted == false);
        if (customerWithPhoneNumber is not null)
        {
            return ResponseModel<AuthenticationResult>.Failure("Phonenumber already in use");

        }
        var passwordSalt = _cryptoService.CreateRandomSalt();
        var sultedPassword = _cryptoService.ComputeSaltedHash(passwordSalt, password);

        var pinSalt = _cryptoService.CreateRandomSalt();
        var sultedPin = _cryptoService.ComputeSaltedHash(pinSalt, pin);
        //var accountNumber = "840"+_cryptoService.GenerateNumericKey(7);
        var accountNumber = "864" + _cryptoService.GenerateNumericKey(7);
        var newReferralCode = _cryptoService.RandomString(8);
        while (await _unitOfWork.CustomerRepository.GetByAsync(x => x.ReferralCode == newReferralCode) != null)
        {
            newReferralCode = _cryptoService.RandomString(8);
        }

        var customer = new Customer()
        {
            PhoneNumber = phoneNumber,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            SaltedHashedPassword = sultedPassword,
            PasswordSalt = passwordSalt,
            SaltedHashedPin = sultedPin,
            PinSalt = pinSalt,
            CreatedBy = email,
            CreatedByIp = ipAddress,
            CreatedDate = _dateTimeProvider.UtcNow,
            AccountNumber = accountNumber,
            BankName = "Wema Bank",
            ReferralCode = newReferralCode

        };



        _unitOfWork.CustomerRepository.Add(customer);

        if (!string.IsNullOrWhiteSpace(referralCode))
        {
            var referringCustomer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.ReferralCode == referralCode);
            if (referringCustomer != null)
            {
                _unitOfWork.ReferralRepository.Add(new Referral
                {
                    ReferredCustomerId = customer.Id,
                    ReferredCustomer = customer,
                    ReferrerId = referringCustomer.Id,
                    CreatedBy = email,
                    CreatedByIp = ipAddress,
                    CreatedDate = _dateTimeProvider.UtcNow,
                });
            }
        }

        var wallet = new Wallet()
        {
            Balance = 0,
            CreatedBy = email,
            CreatedByIp = "::1",
            CreatedDate = _dateTimeProvider.UtcNow,
            Status = "ACTIVE",
            CustomerId = customer.Id,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber

        };



        _unitOfWork.WalletRepository.Add(wallet);
        await _unitOfWork.Complete();
        var token = _jwtTokenGenerator.GenerateToken(customer.Id, customer.FirstName, customer.LastName);
        var customerrDTO = _mapper.Map<CustomerDTO>(customer);

        var resfreshToken = GenerateRefreshToken();
        customer.RefreshToken = resfreshToken;
        customer.RefreshTokenExpiryTime = _dateTimeProvider.UtcNow.AddDays(6);

        _unitOfWork.CustomerRepository.Update(customer);

        var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.SignUp);
        if (emailTemplate != null)
        {
            var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
            await _BerachahThirdPartyService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", customer.FullName).Replace("[ACCOUNT_NUMBER]", accountNumber));
        }




        wallet.CheckSum = wallet.GetCheckSum();
        wallet.ModifiedBy = customer.FullName;
        wallet.ModifiedByIp = "::0";
        wallet.ModifiedDate = _dateTimeProvider.UtcNow;
        _unitOfWork.WalletRepository.Update(wallet);
        await _unitOfWork.Complete();

        return ResponseModel<AuthenticationResult>.Success(new AuthenticationResult(customerrDTO, token, resfreshToken));
    }


    public async Task<ResponseModel<string>> SendForgotPasswordVerificationCode(string phoneNumber, string ipAddress, CancellationToken cancellationToken)
    {
        try
        {
            var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.PhoneNumber == phoneNumber || x.Email == phoneNumber && x.IsDeleted == false);
            if (customer is null)
            {
                return ResponseModel<string>.Failure("customer not found");
            }


            var otp = await _otpService.CreateAsync(phoneNumber, ResourceConstants.ForgotPassword);
            if (!otp.IsSuccessful || otp.Data == null)
                throw new ErrorSavingRecordException();

            string otpHash = _cryptoService.EncryptText(otp.Data.Id);

            //var smsTemplate = _unitOfWork.SmsTemplateRepository.GetByAsync(x => x.SmsType == SmsType.)
            var msge = $"Please use {otp.Data.Code} to complete your transaction";
            _BerachahThirdPartyService.SendSms(phoneNumber, $"Please use {otp.Data.Code} to complete your transaction", _appSettings.SmsAccountNumber, _appSettings.SmsAccountId);
            _BerachahThirdPartyService.SendEmail(customer.Email, "OTP", msge);
            return ResponseModel<string>.Success(otpHash);
        }
        catch (Exception ex)
        {

            _logger.LogCritical(ex.Message);
            return ResponseModel<string>.Failure("An error has ocure, please try again later");
        }

    }

    public async Task<ResponseModel<string>> VerifyForgotPasswordCode(string code, string hash)
    {
        var hashId = _cryptoService.DecryptText(hash);


        var security = await _otpService.GetSingleAsync(hashId);
        if (!security.IsSuccessful || security.Data == null)
            throw new ResourceNotFoundException("Verification code error");


        if (security.Data.Code != code)
            return ResponseModel<string>.Failure("Invalid activation code");

        if (security.Data.IsUsed)
            return ResponseModel<string>.Failure("Activation code already used");

        var use = await _otpService.Use(hashId);

        if (!use.IsSuccessful || use.Data == null)
            return ResponseModel<string>.Failure(use.Message);

        return ResponseModel<string>.Success(hash);
    }


    public async Task<ResponseModel<bool>> ResetPassword(string email, string ConfirmPassword, string password, string hash)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Email == email && x.IsDeleted == false);
        if (customer is null)
        {
            return ResponseModel<bool>.Failure("Customer not found");
        }

        var hashId = _cryptoService.DecryptText(hash);

        var security = await _otpService.GetSingleAsync(hashId);
        if (!security.IsSuccessful || security.Data == null)
            throw new ResourceNotFoundException("Security code not found");

        if (!security.Data.IsUsed)
            return ResponseModel<bool>.Failure("Customer not validated");

        if (security.Data.ResourceId != customer.PhoneNumber)
        {
            if (security.Data.ResourceId != customer.Email)
            {
                return ResponseModel<bool>.Failure("Customer not validated");
            }
        }


        if (!password.Equals(ConfirmPassword))
        {
            return ResponseModel<bool>.Failure("Password mismatch");
        }

        var saltedPassword = _cryptoService.ComputeSaltedHash(customer.PasswordSalt, password);
        customer.PasswordTries = 0;
        customer.SaltedHashedPassword = saltedPassword;
        customer.ModifiedBy = "self";
        customer.ModifiedDate = _dateTimeProvider.UtcNow;
        _unitOfWork.CustomerRepository.Update(customer);
        await _unitOfWork.Complete();

        var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.ForgotPassword);
        if (emailTemplate != null)
        {
            var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
            _BerachahThirdPartyService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", customer.FullName));
        }
        return ResponseModel<bool>.Success(true, "You have successfully changed your password");

    }

    public async Task<ResponseModel<bool>> ResetUserPassword(string email, string confirmPassword, string password)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return ResponseModel<bool>.Failure("User account not found");

            var removeAsync = await _userManager.RemovePasswordAsync(user);

            if (!removeAsync.Succeeded)
            {
                return ResponseModel<bool>.Failure("An error has occurred!");
            }
            var addAsync = await _userManager.AddPasswordAsync(user, confirmPassword);

            if (!addAsync.Succeeded)
            {
                return ResponseModel<bool>.Failure("An error has occurred!");
            }

            //return ResponseModel<bool>.Success(true, "Password Reset Successful");

            var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.ForgotPassword);
            if (emailTemplate != null)
            {
                var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
                var fullname = $"{user.LastName} {user.FirstName}";
                _BerachahThirdPartyService.SendEmail(user.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", fullname));
            }
            return ResponseModel<bool>.Success(true, "You have successfully changed your password");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return ResponseModel<bool>.Failure("An error has ocured, please try again later");
        }

    }

    public async Task<ResponseModel<AdminAuthResult>> GetAdminTokenAsync(string email, string password, string ipAddress, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email.Trim().Normalize());
            if (user is null)
            {
                return ResponseModel<AdminAuthResult>.Failure("Invalid email and password");

            }

            if (!user.IsActive)
            {
                return ResponseModel<AdminAuthResult>.Failure("Invalid records");
            }
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                return ResponseModel<AdminAuthResult>.Failure("Invalid email and pasword");
            }

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.LastName);
            var userDto = new UserDTO()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                Id = user.Id,
            }; //= _mapper.Map<UserDTO>(user);

            return ResponseModel<AdminAuthResult>.Success(new AdminAuthResult(userDto, token));
        }
        catch (Exception ex)
        {

            _logger.LogCritical(ex.Message);
            return ResponseModel<AdminAuthResult>.Failure($"An error has ocure, please try again later == > {ex.Message}");
        }
    }

    public async Task<ResponseModel<string>> SendUserForgotPasswordVerificationCode(string email, string ipAddress, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return ResponseModel<string>.Failure("customer not found");
            }


            var otp = await _otpService.CreateAsync(email, ResourceConstants.ForgotPassword);
            if (!otp.IsSuccessful || otp.Data == null)
                throw new ErrorSavingRecordException();

            string otpHash = _cryptoService.EncryptText(otp.Data.Id);

            var msge = $"Please use {otp.Data.Code} to complete your transaction";
            //_BerachahThirdPartyService.SendSms(phoneNumber, $"Please use {code} to complete your transaction");
            _BerachahThirdPartyService.SendEmail(email, "OTP", msge);
            return ResponseModel<string>.Success(otpHash);
        }
        catch (Exception ex)
        {

            _logger.LogCritical(ex.Message);
            return ResponseModel<string>.Failure("An error has ocure, please try again later");
        }
    }



    public async Task<ResponseModel<AccountValidationResponse>> SendAccountNumberVerificationCode(string accountNumber)
    {
        var response = await _bankOneAccountService.GetAccountsByAccountNumber(accountNumber);
        if (response == null || !response.IsSuccessful || response.Data == null)
        {
            throw new ResourceNotFoundException("fetch account error");
        }

        if (!response.IsSuccessful || response.Data == null)
        {
            throw new ResourceNotFoundException(response.Message);
        }
        var otp = await _otpService.CreateAsync(response.Data.PhoneNumber, ResourceConstants.AccountValidation);
        if (!otp.IsSuccessful || otp.Data == null)
            throw new ErrorSavingRecordException();

        string otpHash = _cryptoService.EncryptText(otp.Data.Id);

        var msge = $"Please use {otp.Data.Code} to complete your transaction";
        _BerachahThirdPartyService.SendSms(response.Data.PhoneNumber, msge, _appSettings.SmsAccountNumber, _appSettings.SmsAccountId);
        _BerachahThirdPartyService.SendEmail(response.Data.PhoneNumber, "OTP", msge);

        var fullname = response.Data.Name.Split(' ');
        return ResponseModel<AccountValidationResponse>.Success(new AccountValidationResponse(response.Data.CustomerID, fullname[0], fullname[1], otpHash));

    }

    public async Task<ResponseModel<string>> ValidateAccountNumber(string code, string hash)
    {
        var hashId = _cryptoService.DecryptText(hash);


        var security = await _otpService.GetSingleAsync(hashId);
        if (!security.IsSuccessful || security.Data == null)
            throw new ResourceNotFoundException("Verification code error");


        if (security.Data.Code != code)
            return ResponseModel<string>.Failure("Invalid activation code");

        if (security.Data.IsUsed)
            return ResponseModel<string>.Failure("Activation code already used");

        var use = await _otpService.Use(hashId);

        if (!use.IsSuccessful || use.Data == null)
            return ResponseModel<string>.Failure(use.Message);

        return ResponseModel<string>.Success(hash);
    }

    public async Task<ResponseModel<AuthenticationResult>> RegisterCustomerWithAccount(string email, string password, string pin, string firstName, string lastName, string middleName, string phoneNumber, string accountId, string hash, string ipAddress, CancellationToken cancellationToken)
    {
        var accountResponse = await _bankOneAccountService.GetAccountsByCustomerId(accountId);
        if (accountResponse == null || !accountResponse.IsSuccessful || accountResponse.Data == null)
        {
            throw new ResourceNotFoundException("fetch account error");
        }

        var hashId = _cryptoService.DecryptText(hash);

        var security = await _otpService.GetSingleAsync(hashId);
        if (!security.IsSuccessful || security.Data == null)
            throw new ResourceNotFoundException("Security code not found");

        if (!security.Data.IsUsed)
            return ResponseModel<AuthenticationResult>.Failure("Account number not validated");

        if (security.Data.ResourceId != accountResponse.Data.PhoneNumber)
            return ResponseModel<AuthenticationResult>.Failure("Account number not validated");



        var customerWithEmail = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Email == email && x.IsDeleted == false);
        if (customerWithEmail is not null)
        {
            return ResponseModel<AuthenticationResult>.Failure("Email already in use");

        }
        var customerWithPhoneNumber = await _unitOfWork.CustomerRepository.GetByAsync(x => x.PhoneNumber == phoneNumber && x.IsDeleted == false);
        if (customerWithPhoneNumber is not null)
        {
            return ResponseModel<AuthenticationResult>.Failure("Phonenumber already in use");

        }
        var passwordSalt = _cryptoService.CreateRandomSalt();
        var sultedPassword = _cryptoService.ComputeSaltedHash(passwordSalt, password);

        var pinSalt = _cryptoService.CreateRandomSalt();
        var sultedPin = _cryptoService.ComputeSaltedHash(pinSalt, pin);
        var walletAccountNumber = "864" + _cryptoService.GenerateNumericKey(7);

        var customer = new Customer()
        {
            PhoneNumber = phoneNumber,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            SaltedHashedPassword = sultedPassword,
            PasswordSalt = passwordSalt,
            SaltedHashedPin = sultedPin,
            PinSalt = pinSalt,
            CreatedBy = email,
            CreatedByIp = ipAddress,
            CreatedDate = _dateTimeProvider.UtcNow,
            AccountNumber = walletAccountNumber,
            BankName = "Wema Bank",
            AccountId = accountResponse.Data.CustomerID,
            IsBvnConfirmed = !string.IsNullOrWhiteSpace(accountResponse.Data.Bvn)

        };
        _unitOfWork.CustomerRepository.Add(customer);

        var wallet = new Wallet()
        {
            Balance = 0,
            CreatedBy = email,
            CreatedByIp = "::1",
            CreatedDate = _dateTimeProvider.UtcNow,
            Status = "ACTIVE",
            CustomerId = customer.Id,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber

        };



        _unitOfWork.WalletRepository.Add(wallet);
        await _unitOfWork.Complete();
        var token = _jwtTokenGenerator.GenerateToken(customer.Id, customer.FirstName, customer.LastName);
        var customerrDTO = _mapper.Map<CustomerDTO>(customer);

        var resfreshToken = GenerateRefreshToken();
        customer.RefreshToken = resfreshToken;
        customer.RefreshTokenExpiryTime = _dateTimeProvider.UtcNow.AddDays(6);

        _unitOfWork.CustomerRepository.Update(customer);

        var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.SignUp);
        if (emailTemplate != null)
        {
            var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
            await _BerachahThirdPartyService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", customer.FullName));
        }

        wallet.CheckSum = wallet.GetCheckSum();
        wallet.ModifiedBy = customer.FullName;
        wallet.ModifiedByIp = "::0";
        wallet.ModifiedDate = _dateTimeProvider.UtcNow;
        _unitOfWork.WalletRepository.Update(wallet);
        await _unitOfWork.Complete();

        return ResponseModel<AuthenticationResult>.Success(new AuthenticationResult(customerrDTO, token, resfreshToken));
    }

    public async Task<ResponseModel<AuthenticationResult>> RefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
    {
        var userPrincipal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(token);
        string? customerEmail = userPrincipal.GetEmail();
        var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Email == customerEmail && x.IsDeleted == false);
        if (customer is null)
        {
            return ResponseModel<AuthenticationResult>.Failure("Invalid cretentials");

        }
        if (customer is null)
        {
            return ResponseModel<AuthenticationResult>.Failure("authentication failed");
        }

        if (customer.RefreshToken != refreshToken || customer.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return ResponseModel<AuthenticationResult>.Failure("invalid referesh token");


        }

        var newtoken = _jwtTokenGenerator.GenerateToken(customer.Id, customer.FirstName, customer.LastName);

        var resfreshToken = GenerateRefreshToken();
        customer.RefreshToken = resfreshToken;
        customer.RefreshTokenExpiryTime = _dateTimeProvider.UtcNow.AddDays(6);

        _unitOfWork.CustomerRepository.Update(customer);

        _unitOfWork.CustomerRepository.Update(customer);

        var customerDto = _mapper.Map<CustomerDTO>(customer);
        return ResponseModel<AuthenticationResult>.Success(new AuthenticationResult(customerDto, newtoken, resfreshToken));
    }

    private string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}


