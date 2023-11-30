using Awacash.Application.Promotions.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Promotions.Handler.Commands.AddPromotion
{
    public class AddPromotionCommanHandler : IRequestHandler<AddPromotionCommand, ResponseModel<bool>>
    {
        private readonly IPromotionService _promotionService;

        public AddPromotionCommanHandler(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        public async Task<ResponseModel<bool>> Handle(AddPromotionCommand request, CancellationToken cancellationToken)
        {
            return await _promotionService.CreatePromotion(request.Title, request.Description, request.HasImage, request.Link, request.Base64File);
        }
    }
}
