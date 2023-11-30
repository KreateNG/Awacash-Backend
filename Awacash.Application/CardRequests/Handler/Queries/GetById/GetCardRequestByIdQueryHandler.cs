using System;
using Awacash.Application.CardRequests.DTOs;
using Awacash.Application.CardRequests.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.CardRequests.Handler.Queries.GetById
{
    public class GetCardRequestByIdQueryHandler : IRequestHandler<GetCardRequestByIdQuery, ResponseModel<CardRequestDTO>>
    {
        private readonly ICardRequestService _cardRequestService;
        public GetCardRequestByIdQueryHandler(ICardRequestService cardRequestService)
        {
            _cardRequestService = cardRequestService;
        }

        public async Task<ResponseModel<CardRequestDTO>> Handle(GetCardRequestByIdQuery request, CancellationToken cancellationToken)
        {
            return await _cardRequestService.GetCardRequestByIdAsync(request.Id);
        }
    }
}

