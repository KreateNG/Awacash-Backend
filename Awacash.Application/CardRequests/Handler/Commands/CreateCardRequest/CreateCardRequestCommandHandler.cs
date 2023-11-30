using Awacash.Application.CardRequests.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.CardRequests.Handler.Commands.CreateCardRequest
{
    public class CreateCardRequestCommandHandler : IRequestHandler<CreateCardRequestCommand, ResponseModel<bool>>
    {
        private readonly ICardRequestService _cardRequestService;

        public CreateCardRequestCommandHandler(ICardRequestService cardRequestService)
        {
            _cardRequestService = cardRequestService;
        }

        public async Task<ResponseModel<bool>> Handle(CreateCardRequestCommand request, CancellationToken cancellationToken)
        {
            return await _cardRequestService.CreateCardRequestAsync(request.AccountNumber, request.CardName, request.CardType.Value, request.DeliveryAddress, request.CardConfigId);
        }
    }
}
