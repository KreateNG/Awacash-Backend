using Awacash.Application.CardRequestConfigurations.DTOs;
using Awacash.Application.CardRequestConfigurations.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.CardRequestConfigurations.Handler.Queries.GetCardRequestConfigurationById
{
    public class GetCardRequestConfigurationByIdQueryHandler : IRequestHandler<GetCardRequestConfigurationByIdQuery, ResponseModel<CardRequestConfigurationDTO>>
    {
        private readonly ICardRequestConfigurationService _cardRequestConfigurationService;

        public GetCardRequestConfigurationByIdQueryHandler(ICardRequestConfigurationService cardRequestConfigurationService)
        {
            _cardRequestConfigurationService = cardRequestConfigurationService;
        }

        public async Task<ResponseModel<CardRequestConfigurationDTO>> Handle(GetCardRequestConfigurationByIdQuery request, CancellationToken cancellationToken)
        {
            return await _cardRequestConfigurationService.GetCardRequestConfigurationByIdAsync(request.Id);
        }
    }
}
