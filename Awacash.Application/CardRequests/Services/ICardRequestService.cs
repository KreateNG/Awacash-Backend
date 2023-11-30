using Awacash.Application.CardRequests.DTOs;
using Awacash.Application.CardRequests.FilterModels;
using Awacash.Domain.Enums;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.CardRequests.Services
{
    public interface ICardRequestService
    {
        Task<ResponseModel<CardRequestDTO>> GetCardRequestByIdAsync(string id);
        Task<ResponseModel<List<CardRequestDTO>>> GetAllCardRequestAsync();
        Task<ResponseModel<PagedResult<CardRequestDTO>>> GetPagedCardRequestAsync(CardRequestFilterModel filterModel);
        Task<ResponseModel<bool>> CreateCardRequestAsync(string accountNumber, string cardName, CardType cardType, string DeliveryAddress, string cardConfigId);

    }
}
