using System;
using Awacash.Application.CardRequests.DTOs;
using Awacash.Application.CardRequests.Services;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;

namespace Awacash.Application.CardRequests.Handler.Queries.GetPagedList
{
    public class GetPaginatedCardRequestQueryHandler : IRequestHandler<GetPaginatedCardRequestQuery, ResponseModel<PagedResult<CardRequestDTO>>>
    {
        private readonly ICardRequestService _cardRequestService;
        public GetPaginatedCardRequestQueryHandler(ICardRequestService cardRequestService)
        {
            _cardRequestService = cardRequestService;
        }

        public async Task<ResponseModel<PagedResult<CardRequestDTO>>> Handle(GetPaginatedCardRequestQuery request, CancellationToken cancellationToken)
        {
            return await _cardRequestService.GetPagedCardRequestAsync(request);
        }
    }
}

