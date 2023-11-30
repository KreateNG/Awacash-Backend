using Awacash.Application.Promotions.DTOs;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Promotions.Handler.Queries.GetPromotionById
{
    public record GetPromotionByIdQuery(string Id):IRequest<ResponseModel<PromotionDTO>>;
}
