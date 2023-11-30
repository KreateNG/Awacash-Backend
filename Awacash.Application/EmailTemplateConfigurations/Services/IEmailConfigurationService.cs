using System;
using Awacash.Application.EmailTemplateConfigurations.DTOs;
using Awacash.Domain.Enums;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;

namespace Awacash.Application.EmailTemplateConfigurations.Services
{
	public interface IEmailConfigurationService
    {
        Task<ResponseModel<EmailTemplateDto>> CreateEmailTemplateAsync(string body, string subject, EmailType emailType);
        Task<ResponseModel<List<EmailTemplateDto>>> GetAllEmailTemplateAsync();
        Task<ResponseModel<EmailTemplateDto>> GetEmailTemplateByIdAsync(string id);
        Task<ResponseModel<PagedResult<EmailTemplateDto>>> GetPaginatedEmailTemplateAsync();
        Task<ResponseModel<bool>> UpdateEmailTemplateAsync(string id, string body, string subject);
    }
}

