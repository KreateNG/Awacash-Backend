using Awacash.Application.Promotions.DTOs;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Promotions.Handler.Queries.GetAllPromotion
{
    public record GetAllPromotionQuery():IRequest<ResponseModel<List<PromotionDTO>>>;
}
