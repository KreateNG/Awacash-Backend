using Awacash.Application.CardRequestConfigurations.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.CardRequestConfigurations.Handler.Commands.CreateCardRequestConfiguration
{
    public class CreateCardRequestConfigurationCommandHandler : IRequestHandler<CreateCardRequestConfigurationCommand, ResponseModel<bool>>
    {
        private readonly ICardRequestConfigurationService _cardRequestConfigurationService;
        public CreateCardRequestConfigurationCommandHandler(ICardRequestConfigurationService cardRequestConfigurationService)
        {
            _cardRequestConfigurationService = cardRequestConfigurationService;
        }
        public async Task<ResponseModel<bool>> Handle(CreateCardRequestConfigurationCommand request, CancellationToken cancellationToken)
        {
            return await _cardRequestConfigurationService.CreateCardRequestConfigurationAsync(request.IssuerName, request.Price, request.CardType);
        }
    }
}
