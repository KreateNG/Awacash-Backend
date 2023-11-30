using Awacash.Application.Promotions.DTOs;
using Awacash.Application.Promotions.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Promotions.Handler.Queries.GetAllPromotion
{
    public class GetAllPromotionQueryHandler : IRequestHandler<GetAllPromotionQuery, ResponseModel<List<PromotionDTO>>>
    {
        private readonly IPromotionService _promotService;

        public GetAllPromotionQueryHandler(IPromotionService promotService)
        {
            _promotService = promotService;
        }

        public async Task<ResponseModel<List<PromotionDTO>>> Handle(GetAllPromotionQuery request, CancellationToken cancellationToken)
        {
            return await _promotService.GetAllPromotion();
        }
    }
}
