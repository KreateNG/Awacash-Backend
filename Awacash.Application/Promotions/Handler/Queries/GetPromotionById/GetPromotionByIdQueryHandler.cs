using Awacash.Application.Promotions.DTOs;
using Awacash.Application.Promotions.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Promotions.Handler.Queries.GetPromotionById
{
    public class GetPromotionByIdQueryHandler : IRequestHandler<GetPromotionByIdQuery, ResponseModel<PromotionDTO>>
    {
        private readonly IPromotionService _promotionService;

        public GetPromotionByIdQueryHandler(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        public async Task<ResponseModel<PromotionDTO>> Handle(GetPromotionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _promotionService.GetPromotionById(request.Id);
        }
    }
}
