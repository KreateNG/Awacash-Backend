using AutoMapper;
using Awacash.Application.AuditLogs.DTOs;
using Awacash.Application.Authentication.Handler.Commands.ChangePassword;
using Awacash.Application.Authentication.Handler.Commands.PoneVerification;
using Awacash.Application.Authentication.Handler.Commands.Register;
using Awacash.Application.Authentication.Handler.Commands.RegisterAccount;
using Awacash.Application.Beneficiaries.DTOs;
using Awacash.Application.BillPayment.Handler.Commands.AirTimePurchase;
using Awacash.Application.CardRequestConfigurations.DTOs;
using Awacash.Application.CardRequests.DTOs;
using Awacash.Application.Common.Model;
using Awacash.Application.Customers.Handler.Commands.ChangePin;
using Awacash.Application.Customers.Handler.Commands.RegisterkMobileDevice;
using Awacash.Application.Customers.Handler.Commands.SetPin;
using Awacash.Application.Customers.Handler.Commands.ValidateBvn;
using Awacash.Application.DisputeLogs.DTOs;
using Awacash.Application.DisputeLogs.Handler.Commands.CreateDispuste;
using Awacash.Application.EmailTemplateConfigurations.DTOs;
using Awacash.Application.EmailTemplateConfigurations.Handler.Commands.CreateEmailTemplate;
using Awacash.Application.FeeConfigurations.DTOs;
using Awacash.Application.FeeConfigurations.Handler.Commands.CreateFeeConfiguration;
using Awacash.Application.Promotions.DTOs;
using Awacash.Application.Promotions.Handler.Commands.AddPromotion;
using Awacash.Application.Role.DTOs;
using Awacash.Application.Role.Handler.Commands.CreateRole;
using Awacash.Application.Savings.DTOs;
using Awacash.Application.Savings.Handler.Commands.CreateSaving;
using Awacash.Application.SavingsConfiguration.DTOs;
using Awacash.Application.SavingsConfiguration.Handler.Commands.CreateSavingConfiguration;
using Awacash.Application.SmsTemplateConfigurations.DTOs;
using Awacash.Application.Transactions.DTOs;
using Awacash.Application.Transactions.Handlers.Commands.AccountTransactionNotification;
using Awacash.Application.Transfers.Handlers.Commands.InterBankTransfer;
using Awacash.Application.Transfers.Handlers.Commands.LocalTransfer;
using Awacash.Application.Transfers.Handlers.Commands.NipNameEnquiry;
using Awacash.Application.Users.Handler.Commands.CreateUser;
using Awacash.Application.Wallets.DTOs;
using Awacash.Contracts.Authentication;
using Awacash.Contracts.BillPayment;
using Awacash.Contracts.Customers;
using Awacash.Contracts.DisputeLogs;
using Awacash.Contracts.EmailConfigurations;
using Awacash.Contracts.FeeConfigurations;
using Awacash.Contracts.Promotions;
using Awacash.Contracts.Roles;
using Awacash.Contracts.Savings;
using Awacash.Contracts.SavingsConfiguration;
using Awacash.Contracts.Transactions;
using Awacash.Contracts.Users;
using Awacash.Domain.Entities;
using Awacash.Domain.IdentityModel;
using Awacash.Shared.Models.Paging;
using AwaCash.Application.Common.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Config();
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod("Mapping")
                    ?? type.GetInterface("IMapFrom`1").GetMethod("Mapping");

                methodInfo?.Invoke(instance, new object[] { this });
            }
        }

        private void Config()
        {
            CreateMap<ChangePinRequest, ChangePinCommand>().ReverseMap();
            CreateMap<SetPinRequest, SetPinCommand>().ReverseMap();
            CreateMap<ChangePasswordRequest, ChangePasswordCommand>().ReverseMap();
            CreateMap<RegisterRequest, RegisterCommand>().ReverseMap();

            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<PagedResult<Customer>, PagedResult<CustomerDTO>>().ReverseMap();

            CreateMap<VerifyPhoneNumberRequest, VerifyPhoneCommand>().ReverseMap();
            CreateMap<SendPhoneNumberVerificationCodeRequest, SendPhoneVerificationCodeCommand>().ReverseMap();
            CreateMap<LocalTransferRequest, LocalTransferCommand>().ReverseMap();
            CreateMap<InterBankTransferRequest, InterBankTransferCommand>().ReverseMap();
            CreateMap<NipNameEnquiryRequest, NipNameEnquiryCommand>().ReverseMap();
            CreateMap<AccountTransactionNotificationRequest, AccountTransactionNotificationCommad>().ReverseMap();
            CreateMap<ApplicationRole, RoleDTO>().ReverseMap();
            CreateMap<CreateRoleRequest, CreateRoleCommand>().ReverseMap();
            CreateMap<PagedResult<ApplicationRole>, PagedResult<RoleDTO>>().ReverseMap();

            CreateMap<PagedResult<Saving>, PagedResult<SavingDTO>>().ReverseMap();
            CreateMap<Saving, SavingDTO>().ReverseMap();
            CreateMap<CreateSavingRequest, CreateSavingCommand>().ReverseMap();
            CreateMap<CreateSavingConfigurationRequest, CreateSavingConfigurationCommand>().ReverseMap();
            CreateMap<CardRequestConfiguration, CardRequestConfigurationDTO>().ReverseMap();
            CreateMap<SavingConfiguration, SavingConfigurationDTO>().ReverseMap();
            CreateMap<SavingConfiguration, SavingConfigurationDTO>().ReverseMap();
            CreateMap<CreateUserRequest, CreateUserCommand>().ReverseMap();
            CreateMap<Transaction, TransactionDTO>().ReverseMap();
            CreateMap<PagedResult<Transaction>, PagedResult<TransactionDTO>>().ReverseMap();
            CreateMap<Promotion, PromotionDTO>().ReverseMap();
            CreateMap<Beneficiary, BeneficiaryDTO>().ReverseMap();
            CreateMap<DisputeLog, DisputeLogDTO>().ReverseMap();
            CreateMap<CreateDipusteLogRequest, CreateDisputeCommand>().ReverseMap();
            CreateMap<ValidateBvnCommand, ValidateCustomerBvnRequest>().ReverseMap();
            CreateMap<AddPromotionCommand, AddPromotionRequest>().ReverseMap();
            CreateMap<RegisterkMobileDeviceRequest, RegisterkMobileDeviceCommand>().ReverseMap();
            CreateMap<SmsTemplate, SmsTemplateDto>().ReverseMap();
            CreateMap<EmailTemplate, EmailTemplateDto>().ReverseMap();
            CreateMap<CreateEmailConfigurationRequest, CreateEmailTemplateCommand>().ReverseMap();
            CreateMap<AirTimePurchaseRequest, AirTimePurchaseCommand>().ReverseMap();


            CreateMap<CreateFeeConfigurationRequest, CreateFeeConfigurationCommand>().ReverseMap();
            CreateMap<FeeConfiguration, FeeConfigurationDto>().ReverseMap();
            CreateMap<Wallet, WalletDTO>().ReverseMap();
            CreateMap<PagedResult<Wallet>, PagedResult<WalletDTO>>().ReverseMap();

            CreateMap<AuditLog, AuditLogDto>().ReverseMap();
            CreateMap<PagedResult<AuditLog>, PagedResult<AuditLogDto>>().ReverseMap();

            CreateMap<CardRequest, CardRequestDTO>().ReverseMap();
            CreateMap<PagedResult<CardRequest>, PagedResult<CardRequestDTO>>().ReverseMap();
            CreateMap<RegisterAccountRequest, RegisterAccountCommand>().ReverseMap();
            CreateMap<OtpResponseDTO, Token>().ReverseMap();
            CreateMap<ReferralDTO, Referral>().ReverseMap();


        }
    }
}
