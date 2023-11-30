using System;
using Awacash.Application.Savings.DTOs;
using Awacash.Application.SmsTemplateConfigurations.DTOs;
using Awacash.Domain.Enums;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;

namespace Awacash.Application.SmsTemplateConfigurations.Services
{
	public interface ISmsConfigurationService
	{
        Task<ResponseModel<SmsTemplateDto>> CreateSmsTemplateAsync(string message, SmsType smsType);
        Task<ResponseModel<List<SmsTemplateDto>>> GetAllSmsTemplateAsync();
        Task<ResponseModel<SmsTemplateDto>> GetSmsTemplateByIdAsync(string id);
        Task<ResponseModel<PagedResult<SmsTemplateDto>>> GetPaginatedSmsTemplateAsync();
        Task<ResponseModel<bool>> UpdateSmsTemplateAsync(string id, string message);
    }
}

