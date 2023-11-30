using AutoMapper;
using Awacash.Application.Common.Exceptions;
using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Application.Common.Model;
using Awacash.Application.Customers.DTOs;
using Awacash.Application.Customers.FilterModels;
using Awacash.Application.Customers.Specifications;
using Awacash.Application.Role.DTOs;
using Awacash.Application.Role.Specifications;
using Awacash.Domain.Entities;
using Awacash.Domain.Interfaces;
using Awacash.Domain.Models.Customer;
using Awacash.Domain.Models.Transactions;
using Awacash.Domain.Settings;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using AwaCash.Application.Common.Exceptions;
using AwaCash.Application.Common.Interfaces.Services;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AwaCash.Shared.Constants.TokenResourceConstants;

namespace Awacash.Application.Customers.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICryptoService _cryptoService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAwacashThirdPartyService _awacashThirdPartyService;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IBankOneAccountService _bankOneAccountService;
        private readonly IOtpService _otpService;

        public CustomerService(ICurrentUser currentUser, IUnitOfWork unitOfWork, ICryptoService cryptoService, IDateTimeProvider dateTimeProvider, ILogger<CustomerService> logger, IAwacashThirdPartyService awacashThirdPartyService, IMapper mapper, IOptions<AppSettings> appSettings, IBankOneAccountService bankOneAccountService, IOtpService otpService)
        {
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
            _cryptoService = cryptoService;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
            _awacashThirdPartyService = awacashThirdPartyService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _bankOneAccountService = bankOneAccountService;
            _otpService = otpService;
        }

        public async Task<ResponseModel<bool>> ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                var userId = _currentUser.GetCustomerId();
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == userId && x.IsDeleted == false);
                if (customer is null)
                {
                    return ResponseModel<bool>.Failure("Customer not found");
                }

                if (_cryptoService.ComputeSaltedHash(customer.PasswordSalt, oldPassword) != customer.SaltedHashedPassword)
                {
                    customer.PasswordTries = customer.PasswordTries++;
                    customer.ModifiedBy = "self";
                    customer.ModifiedDate = _dateTimeProvider.UtcNow;
                    _unitOfWork.CustomerRepository.Update(customer);
                    await _unitOfWork.Complete();
                    return ResponseModel<bool>.Failure("Invalid password you 5 more tries");
                }
                var saltedPassword = _cryptoService.ComputeSaltedHash(customer.PasswordSalt, newPassword);
                customer.PasswordTries = 0;
                customer.SaltedHashedPassword = saltedPassword;
                customer.ModifiedBy = "self";
                customer.ModifiedDate = _dateTimeProvider.UtcNow;
                _unitOfWork.CustomerRepository.Update(customer);
                await _unitOfWork.Complete();


                return ResponseModel<bool>.Success(true, "You have successfully changed your password");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<bool>.Failure("An error has ocure, please try again later");
            }

        }

        public async Task<ResponseModel<bool>> ChangePin(string oldPin, string newPin)
        {
            var userId = _currentUser.GetCustomerId();
            var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == userId && x.IsDeleted == false);
            if (customer is null)
            {
                return ResponseModel<bool>.Failure("Customer not found");
            }

            if (_cryptoService.ComputeSaltedHash(customer.PinSalt.Value, oldPin) != customer.SaltedHashedPin)
            {
                customer.PinTries = customer.PinTries++;
                customer.ModifiedBy = "self";
                customer.ModifiedDate = _dateTimeProvider.UtcNow;
                _unitOfWork.CustomerRepository.Update(customer);
                await _unitOfWork.Complete();
                return ResponseModel<bool>.Failure("Invalid pin you 5 more tries");
            }

            var saltedPin = _cryptoService.ComputeSaltedHash(customer.PinSalt.Value, newPin);
            customer.PinTries = 0;
            customer.SaltedHashedPin = saltedPin;
            customer.ModifiedBy = "self";
            customer.ModifiedDate = _dateTimeProvider.UtcNow;
            _unitOfWork.CustomerRepository.Update(customer);
            await _unitOfWork.Complete();
            return ResponseModel<bool>.Success(true, "You have successfully changed your pin");

        }

        public async Task<ResponseModel<bool>> SetPin(string pin)
        {
            var userId = _currentUser.GetUserId().ToString();
            var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == userId && x.IsDeleted == false);
            if (customer is null)
            {
                return ResponseModel<bool>.Failure("Customer not found");
            }
            var pinSalt = _cryptoService.CreateRandomSalt();
            var saltedPin = _cryptoService.ComputeSaltedHash(pinSalt, pin);
            customer.PinSalt = pinSalt;
            customer.SaltedHashedPin = saltedPin;
            customer.ModifiedBy = "self";
            customer.ModifiedDate = _dateTimeProvider.UtcNow;
            _unitOfWork.CustomerRepository.Update(customer);
            await _unitOfWork.Complete();

            return ResponseModel<bool>.Success(true, "Your have set your pin successfully");

        }


        public async Task<ResponseModel<string>> SendPhoneNumberVerificationCode(string phoneNumber)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.PhoneNumber == phoneNumber && x.IsDeleted == false);
            if (customer is not null)
            {
                return ResponseModel<string>.Failure("Phone number is taken by another user");
            }

            var otp = await _otpService.CreateAsync(phoneNumber, ResourceConstants.AccountValidation);
            if (!otp.IsSuccessful || otp.Data == null)
                throw new ErrorSavingRecordException();

            string otpHash = _cryptoService.EncryptText(otp.Data.Id);
            await _awacashThirdPartyService.SendSms(phoneNumber, $"Please use {otp.Data.Code} to complete your transaction", _appSettings.SmsAccountNumber, _appSettings.SmsAccountId);
            return ResponseModel<string>.Success(otpHash);

        }

        public async Task<ResponseModel<string>> VerificationPhoneNumber(string code, string hash)
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

        public async Task<ResponseModel<List<CustomerDTO>>> GetAllCustomersAsync()
        {
            var customers = await _unitOfWork.CustomerRepository.ListAllAsync();
            return ResponseModel<List<CustomerDTO>>.Success(_mapper.Map<List<CustomerDTO>>(customers));
        }

        public async Task<ResponseModel<PagedResult<CustomerDTO>>> GetPaginatedCustomerAsync(CustomerFilterModel filterModel)
        {
            var customersspecification = new CustomerFilterSpecification(firstname: filterModel.FirstName, lastname: filterModel.LastName, middlename: filterModel.MiddleName, email: filterModel.Email, phonenumber: filterModel.PhoneNumber);
            var customers = await _unitOfWork.CustomerRepository.ListAsync(filterModel.PageIndex, filterModel.PageSize, customersspecification);
            return ResponseModel<PagedResult<CustomerDTO>>.Success(_mapper.Map<PagedResult<CustomerDTO>>(customers));
        }

        public async Task<ResponseModel<CustomerDTO>> GetCustomerByIdAsync(string customerId)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);
            if (customer is null)
            {
                return ResponseModel<CustomerDTO>.Failure("Specified role identifier was not found");
            }
            return ResponseModel<CustomerDTO>.Success(_mapper.Map<CustomerDTO>(customer));
        }

        public async Task<ResponseModel<CustomerAccountBalanceDTO>> GetCustomerBalanceAsync()
        {
            CustomerAccountBalanceDTO response = null;
            var userId = _currentUser.GetCustomerId();
            var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == userId && x.IsDeleted == false, x => x.Wallet);
            if (customer is null)
            {
                return ResponseModel<CustomerAccountBalanceDTO>.Failure("Customer not found");
            }
            if (customer.Wallet is null)
            {
                return ResponseModel<CustomerAccountBalanceDTO>.Failure("wallet not found");
            }

            return ResponseModel<CustomerAccountBalanceDTO>.Success(new CustomerAccountBalanceDTO(customer.FullName, customer.Wallet.Balance.Value));
        }

        public async Task<ResponseModel<string>> UploadCustomerProfileImageAsync(string passwordBase64)
        {
            string response = null;
            try
            {
                var userId = _currentUser.GetCustomerId();
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == userId && x.IsDeleted == false);
                if (customer is null)
                {
                    return ResponseModel<string>.Failure("Customer not found");
                }
                var (valid, error, ext) = ValidateImage(passwordBase64);
                if (!valid)
                {
                    return ResponseModel<string>.Failure(error);
                }
                var fileName = $"Awacash_{_dateTimeProvider.UtcNow.ToString("yyyyMMddHHmmss")}_{customer.FullName.Replace(" ", "_")}_{_cryptoService.GetNextInt64().ToString().Substring(0, 4)}.{ext}";
                var target = System.IO.Path.Combine(_appSettings.SystemPath + _appSettings.ProfilePath, fileName);
                await File.WriteAllBytesAsync(target, Convert.FromBase64String(passwordBase64));
                var imageUrl = $"{_appSettings.DomainName}{_appSettings.ProfilePath}/{fileName}";


                customer.ProfileImageUrl = imageUrl;
                customer.ModifiedBy = customer.FullName;
                customer.ModifiedByIp = "::1";
                customer.ModifiedDate = _dateTimeProvider.UtcNow;
                _unitOfWork.CustomerRepository.Update(customer);
                await _unitOfWork.Complete();
                return ResponseModel<string>.Success(imageUrl);
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while getting customer balance: {ex.Message}", nameof(GetCustomerByIdAsync));
                return ResponseModel<string>.Failure(ex.Message);
            }
        }

        public async Task<ResponseModel<string>> InitializeBvnAuthenticationAsync(string Bvn)
        {
            var userId = _currentUser.GetCustomerId();
            var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == userId && x.IsDeleted == false);
            if (customer is null)
            {
                return ResponseModel<string>.Failure("Customer not found");
            }

            customer.Bvn = Bvn;
            customer.ModifiedDate = _dateTimeProvider.UtcNow;
            _unitOfWork.CustomerRepository.Update(customer);
            await _unitOfWork.Complete();
            return await _awacashThirdPartyService.InitializeBvnAuth();
        }

        public async Task<ResponseModel<BvnCustomerInfo>> GetBvnCustomerInfoWithAccessCode(string accessCode)
        {
            return await _awacashThirdPartyService.GetBvnCustomerInfoWithAccessCode(accessCode);
        }


        public async Task<ResponseModel<bool>> ValidateCustomerBvn(string firstName, string lastName, DateTime dateOfBirth, string accessToken)
        {
            try
            {

                var bvnInfo = await _awacashThirdPartyService.GetBvnCustomerInfoWithAccessCode(accessToken);
                var customerid = _currentUser.GetCustomerId();

                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerid);
                if (customer == null)
                {
                    return ResponseModel<bool>.Failure("Customer not found");
                }




                if (!bvnInfo.IsSuccessful)
                {
                    return ResponseModel<bool>.Failure(bvnInfo.Message);
                }
                if (bvnInfo.Data == null)
                {
                    return ResponseModel<bool>.Failure("Fail to validate BVN, Please try again");
                }
                var customerInfo = bvnInfo.Data;

                //var a1 = customerInfo?.first_name?.Trim().ToUpper() == firstName.Trim().ToUpper();
                //var a2 = customerInfo?.surname?.Trim().ToUpper() == lastName.Trim().ToUpper();
                //var a3 = (customerInfo?.DateOfBirth?.Date.Day + 1) == dateOfBirth.Date.Day;
                //var a4 = customerInfo?.DateOfBirth?.Date.Month == dateOfBirth.Date.Month;
                //var a5 = customerInfo?.DateOfBirth?.Date.Year == dateOfBirth.Date.Year;
                var bvndate = DateTimeOffset.Parse(customerInfo.DateOfBirth);

                if (customerInfo?.first_name?.Trim().ToUpper() == firstName.Trim().ToUpper() && customerInfo?.surname?.Trim().ToUpper() == lastName.Trim().ToUpper() && bvndate.Day == dateOfBirth.Date.Day && bvndate.Date.Month == dateOfBirth.Date.Month && bvndate.Date.Year == dateOfBirth.Date.Year)
                {
                    customer.IsBvnConfirmed = true;
                    customer.FirstName = customerInfo.first_name;
                    customer.LastName = customerInfo.surname;
                    customer.ModifiedBy = customer.FullName;
                    customer.ModifiedDate = _dateTimeProvider.UtcNow;
                    customer.ModifiedByIp = "::1";
                    _unitOfWork.CustomerRepository.Update(customer);
                    await _unitOfWork.Complete();

                    var newBvnInfo = new BvnInfo()
                    {
                        Title = bvnInfo.Data.title,
                        FirstName = bvnInfo.Data.first_name?.Trim(),
                        MiddleName = bvnInfo.Data.middle_name?.Trim(),
                        Surname = bvnInfo.Data.surname?.Trim(),
                        AccountDetailId = bvnInfo.Data.AccountDetailId,
                        BranchName = bvnInfo.Data.branch_name,
                        DateOfBirth = bvndate.DateTime,
                        Email = bvnInfo.Data.email,
                        EnrollBankCode = bvnInfo.Data.enroll_bank_code,
                        EnrollmentDate = bvnInfo.Data.enrollment_date,
                        Gender = bvnInfo.Data.gender,
                        ImageDetailsId = bvnInfo.Data.ImageDetailsId,
                        LgaOfCapture = bvnInfo.Data.lga_of_capture,
                        LgaOfOrigin = bvnInfo.Data.lga_of_origin,
                        LgaOfResidence = bvnInfo.Data.lga_of_residence,
                        MaritalStatus = bvnInfo.Data.marital_status,
                        NameOnCard = bvnInfo.Data.name_on_card,
                        Nationality = bvnInfo.Data.nationality,
                        Nin = bvnInfo.Data.nin,
                        PhoneNumber1 = bvnInfo.Data.Phone_number1,
                        PhoneNumber2 = bvnInfo.Data.phone_number2,
                        StateOfCapture = bvnInfo.Data.state_of_capture,
                        StateOfOrigin = bvnInfo.Data.state_of_origin,
                        StateOfResidence = bvnInfo.Data.state_of_residence,
                        CreatedBy = "",
                        CreatedByIp = "::1",
                        CreatedDate = _dateTimeProvider.UtcNow
                    };

                    _unitOfWork.BvnInfoRepository.Add(newBvnInfo);
                    await _unitOfWork.Complete();

                    if (string.IsNullOrWhiteSpace(customer.AccountId))
                    {
                        var nunBanAccountRes = await _bankOneAccountService.AccountOpening(bvnInfo.Data.first_name, bvnInfo.Data.surname, bvnInfo.Data.middle_name, "", bvnInfo.Data.Phone_number1, bvnInfo.Data.gender.ToLower(), "", bvnInfo.Data.DateOfBirth.ToString(), "", bvnInfo.Data.nin, customer.Email, "", "", "", "");

                        if (nunBanAccountRes == null || !nunBanAccountRes.IsSuccessful || nunBanAccountRes.Data == null)
                        {
                            //customer.AccountId = "P";

                        }
                        else
                        {
                            customer.AccountId = nunBanAccountRes.Data.CustomerID;
                        }
                        customer.ModifiedBy = customer.FullName;
                        customer.ModifiedDate = _dateTimeProvider.UtcNow;
                        customer.ModifiedByIp = "::1";
                        _unitOfWork.CustomerRepository.Update(customer);
                        await _unitOfWork.Complete();
                    }

                    return ResponseModel<bool>.Success(true);
                }
                return ResponseModel<bool>.Failure("Bvn Mismatch");
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while exchanging access code for access token: {ex.Message}", nameof(ValidateCustomerBvn));
                return ResponseModel<bool>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<bool>> RegisterkMobileDevice(string phone, string deviceId, string otp)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.PhoneNumber == phone);
                if (customer == null)
                {
                    return ResponseModel<bool>.Failure("Customer not found");
                }
                var deviceCustomer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.DeviceId == deviceId);
                if (deviceCustomer != null)
                {
                    deviceCustomer.DeviceId = null;
                    deviceCustomer.ModifiedBy = "sys";
                    deviceCustomer.ModifiedDate = _dateTimeProvider.UtcNow;
                    _unitOfWork.CustomerRepository.Update(deviceCustomer);
                }

                customer.DeviceId = deviceId;
                customer.ModifiedBy = "sys";
                customer.ModifiedDate = _dateTimeProvider.UtcNow;
                _unitOfWork.CustomerRepository.Update(customer);

                await _unitOfWork.Complete();

                return ResponseModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while registering mobile device: {ex.Message}", nameof(RegisterkMobileDevice));
                return ResponseModel<bool>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<bool>> UpdateCustomerAddressAndNextOfKin(string address, string state, string city, string nextOfKinName, string nextOfKinRelationship, string nextOfKinPhoneNumber, string country, string NextOfKinEmail, string NextOfKinAddress)
        {
            try
            {
                var customerId = _currentUser.GetCustomerId();
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId && x.IsDeleted == false);
                if (customer is null)
                {
                    return ResponseModel<bool>.Failure("Customer not found");
                }
                customer.Address = address;
                customer.State = state;
                customer.City = city;
                customer.NextOfKinName = nextOfKinName;
                customer.NextOfKinPhoneNumber = nextOfKinPhoneNumber;
                customer.NextOfKinRelationship = nextOfKinRelationship;
                customer.ModifiedBy = customer.Email;
                customer.NextOfKinAddress = NextOfKinAddress;
                customer.NextOfKinEmail = NextOfKinEmail;
                customer.Country = country;
                customer.ModifiedByIp = "::1";
                customer.ModifiedDate = _dateTimeProvider.UtcNow;


                _unitOfWork.CustomerRepository.Update(customer);
                await _unitOfWork.Complete();
                return ResponseModel<bool>.Success(true, "Customer Updated successfuly");
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while updating customer address and next of kin information: {ex.Message}", nameof(RegisterkMobileDevice));
                return ResponseModel<bool>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<List<AccountDTO>>> GetAllCustomerAccountsAsync()
        {
            var customerId = _currentUser.GetCustomerId();
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new UnauthorizedException("Unauthorize");
            }
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);
            if (customer is null)
            {
                throw new ResourceNotFoundException("Customer not found");
            }
            var accounts = new List<AccountDTO>();
            if (string.IsNullOrWhiteSpace(customer.AccountId))
            {
                return ResponseModel<List<AccountDTO>>.Success(accounts);
            }
            var accountsResponse = await _bankOneAccountService.GetAccountsByCustomerId(customer.AccountId);
            if (accountsResponse == null || !accountsResponse.IsSuccessful || accountsResponse.Data == null)
            {
                throw new ResourceNotFoundException("fetch error");
            }
            foreach (var acc in accountsResponse.Data.Accounts)
            {
                accounts.Add(new AccountDTO
                {
                    AccountNumber = acc.Nuban,
                    AccountStatus = acc.AccountStatus,
                    AccountType = acc.AccountType,
                    AvailableBalance = acc.AvailableBalance,
                    LedgerBalance = acc.LedgerBalance,
                    WithdrawableAmount = acc.WithdrawableAmount
                });
            }

            return ResponseModel<List<AccountDTO>>.Success(accounts);
        }

        public async Task<ResponseModel> RequestStatement(string accountNumber, DateTime from, DateTime to)
        {
            var customerId = _currentUser.GetCustomerId();
            var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId && x.IsDeleted == false);
            if (customer is null)
            {
                return ResponseModel.Failure("Customer not found");
            }
            var accountRes = await _bankOneAccountService.GetAccountDetail(accountNumber);
            if (accountRes is null || accountRes.Data == null || !accountRes.Data.IsSuccessful)
            {
                return ResponseModel.Failure("Account verification failed. Please check the details and try again");
            }
            var statementRes = await _bankOneAccountService.RequestStatement(accountNumber, from, to);
            if (statementRes is null || statementRes.Data == null || !statementRes.Data.IsSuccessful)
            {
                return ResponseModel.Failure("Fail to generate account statement. Please try again");
            }

            if (string.IsNullOrWhiteSpace(statementRes.Data.Message))
            {
                return ResponseModel.Failure("Fail to generate account statement. Please try again");
            }
            var files = new Dictionary<string, string>
            {
                { "Customer statemet.pdf", statementRes.Data.Message }
            };

            var body = $"Hi {customer?.FullName},\n\nThe bank statement you requested is attached to this email.\n\nIf you did not request a statement, please send an email to support@awacashmfb.com right away";

            _awacashThirdPartyService.SendEmail(customer.Email, "Account statement", body, files);

            return ResponseModel.Success("Your statement is on its way to your inbox");
        }

        public async Task<ResponseModel<CustomerDTO>> UpdateProfile(string lastname, string firstname, string middlename)
        {
            var customerId = _currentUser.GetCustomerId();
            var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId && x.IsDeleted == false);
            if (customer is null)
            {
                return ResponseModel<CustomerDTO>.Failure("Customer not found");
            }

            customer.FirstName = firstname;
            customer.LastName = lastname;
            customer.MiddleName = middlename;
            customer.ModifiedBy = customer.FullName;
            customer.ModifiedDate = _dateTimeProvider.UtcNow;

            _unitOfWork.CustomerRepository.Update(customer);

            return ResponseModel<CustomerDTO>.Success(_mapper.Map<CustomerDTO>(customer));
        }


        public async Task<BvnVerificationModel> TestCustomerBvn(string firstName, string lastName, DateTime dateOfBirth, string accessToken)
        {
            try
            {

                var bvnInfo = await _awacashThirdPartyService.GetBvnCustomerInfoWithAccessCode(accessToken);

                if (!bvnInfo.IsSuccessful)
                {
                    return null; // ResponseModel<BvnVerificationModel>.Failure(bvnInfo.Message);
                }
                if (bvnInfo.Data == null)
                {
                    return null; // ResponseModel<BvnVerificationModel>.Failure("Fail to validate BVN, Please try again");
                }
                var customerInfo = bvnInfo.Data;

                //var a1 = customerInfo?.first_name?.Trim().ToUpper() == firstName.Trim().ToUpper();
                //var a2 = customerInfo?.surname?.Trim().ToUpper() == lastName.Trim().ToUpper();
                //var a3 = (customerInfo?.DateOfBirth?.Date.Day + 1) == dateOfBirth.Date.Day;
                //var a4 = customerInfo?.DateOfBirth?.Date.Month == dateOfBirth.Date.Month;
                //var a5 = customerInfo?.DateOfBirth?.Date.Year == dateOfBirth.Date.Year;

                var bvndate = DateTimeOffset.Parse(customerInfo.DateOfBirth);
                var obj = new BvnVerificationModel
                {
                    Bnv = new DetealModel
                    {
                        FirstName = customerInfo?.first_name?.Trim().ToUpper(),
                        LastName = customerInfo?.surname?.Trim().ToUpper(),
                        DateOfBirth = bvndate.DateTime,
                        Day = bvndate.Date.Day,
                        Month = bvndate.Date.Month,
                        Year = bvndate.Date.Year
                    },
                    Customer = new DetealModel
                    {
                        FirstName = firstName.Trim().ToUpper(),
                        LastName = lastName.Trim().ToUpper(),
                        DateOfBirth = dateOfBirth,
                        Day = dateOfBirth.Date.Day,
                        Month = dateOfBirth.Date.Month,
                        Year = dateOfBirth.Date.Year
                    }
                };

                return obj; // ResponseModel<BvnVerificationModel>.Success(obj);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while exchanging access code for access token: {ex.Message}", nameof(ValidateCustomerBvn));
                return null; // ResponseModel<BvnVerificationModel>.Failure("Exception error");
            }
        }

        public async Task<int> ProcessRefeerral()
        {
            var customers = await _unitOfWork.CustomerRepository.ListAllAsync();
            foreach (var customer in customers)
            {
                customer.ReferralCode = _cryptoService.RandomString(8);
                _unitOfWork.CustomerRepository.Update(customer);
            }
            return await _unitOfWork.Complete();
        }

        public async Task<ResponseModel<List<ReferralDTO>>> GetReferee()
        {
            var customerId = _currentUser.GetCustomerId();
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);
            if (customer is null)
            {
                throw new ResourceNotFoundException("Customer not found");
            }
            var referrees = await _unitOfWork.ReferralRepository.ListAsync(x => x.ReferrerId == customerId, x => x.ReferredCustomer);
            return ResponseModel<List<ReferralDTO>>.Success(_mapper.Map<List<ReferralDTO>>(referrees));
        }
        #region private method

        private Tuple<bool, string, string> ValidateImage(string base64String, bool isPassport = false)
        {
            bool isValid = false;
            string error = string.Empty;
            string ext = string.Empty;

            if (!ValidateDocumentSize(base64String))
            {
                error = $"Passport size should not be more than 5MB";
                return new Tuple<bool, string, string>(isValid, error, ext);
            }

            if (!ValidateDocumentType(base64String, out ext))
            {
                error = $"Invalid passport file.";
                return new Tuple<bool, string, string>(isValid, error, ext);
            }
            isValid = true;
            return new Tuple<bool, string, string>(isValid, error, ext);

        }

        private bool ValidateDocumentSize(string base64String)
        {
            bool isValid = false;

            var stringLength = base64String.Length - "data:image/png;base64,".Length;
            decimal actualLenght = stringLength / 4;
            var sizeInBytes = Math.Ceiling(actualLenght) * 3;
            var sizeInKb = sizeInBytes / 1000;

            if (sizeInKb < (1024 * 5))
            {
                isValid = true;
            }

            return isValid;
        }

        private bool ValidateDocumentType(string base64String, out string extention)
        {
            bool isValid = false;
            extention = string.Empty;
            var data = base64String.Substring(0, 5);
            if (!string.IsNullOrWhiteSpace(data))
            {
                if (data.ToUpper().Equals("IVBOR") || data.ToUpper().Equals("/9J/4".ToUpper()))
                {
                    extention = data.ToUpper().Equals("IVBOR") ? "png" : "jpg";
                    isValid = true;
                }
                //else
                //{
                //    if (data.ToUpper().Equals("IVBOR") || data.ToUpper().Equals("/9J/4".ToUpper()) || data.ToUpper().Equals("JVBER".ToUpper()))
                //    {
                //        isValid = true;
                //    }
                //}

            }

            return isValid;
        }








        #endregion
    }
}
