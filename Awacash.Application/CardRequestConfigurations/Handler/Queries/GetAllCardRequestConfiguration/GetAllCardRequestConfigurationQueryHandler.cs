using Awacash.Application.CardRequestConfigurations.DTOs;
using Awacash.Application.CardRequestConfigurations.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.CardRequestConfigurations.Handler.Queries.GetAllCardRequestConfiguration
{
    public class GetAllCardRequestConfigurationQueryHandler : IRequestHandler<GetAllCardRequestConfigurationQuery, ResponseModel<List<CardRequestConfigurationDTO>>>
    {
        private readonly ICardRequestConfigurationService _cardRequestConfigurationService;

        public GetAllCardRequestConfigurationQueryHandler(ICardRequestConfigurationService cardRequestConfigurationService)
        {
            _cardRequestConfigurationService = cardRequestConfigurationService;
        }
        public async Task<ResponseModel<List<CardRequestConfigurationDTO>>> Handle(GetAllCardRequestConfigurationQuery request, CancellationToken cancellationToken)
        {
            return await _cardRequestConfigurationService.GetAllCardRequestConfigurationAsync();
        }
    }
}
