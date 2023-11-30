using System;
using Awacash.Application.CardRequests.DTOs;
using Awacash.Application.CardRequests.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.CardRequests.Handler.Queries.GetAll
{
    public class GetAllCardRequestQueryHandler : IRequestHandler<GetAllCardRequestQuery, ResponseModel<List<CardRequestDTO>>>
    {
        private readonly ICardRequestService _cardRequestService;

        public GetAllCardRequestQueryHandler(ICardRequestService cardRequestService)
        {
            _cardRequestService = cardRequestService;
        }

        public async Task<ResponseModel<List<CardRequestDTO>>> Handle(GetAllCardRequestQuery request, CancellationToken cancellationToken)
        {
            return await _cardRequestService.GetAllCardRequestAsync();
        }
    }
}

