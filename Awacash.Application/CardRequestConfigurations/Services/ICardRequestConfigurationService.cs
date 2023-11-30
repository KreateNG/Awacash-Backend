using Awacash.Application.CardRequestConfigurations.DTOs;
using Awacash.Domain.Enums;
using Awacash.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.CardRequestConfigurations.Services
{
    public interface ICardRequestConfigurationService
    {
        Task<ResponseModel<bool>> CreateCardRequestConfigurationAsync(string IssuerName, decimal price, CardType cardType);
        Task<ResponseModel<bool>> UpdateCardRequestConfigurationAsync(string id, string IssuerName, decimal price);
        Task<ResponseModel<CardRequestConfigurationDTO>> GetCardRequestConfigurationByIdAsync(string id);
        Task<ResponseModel<List<CardRequestConfigurationDTO>>> GetAllCardRequestConfigurationAsync();

    }
}
