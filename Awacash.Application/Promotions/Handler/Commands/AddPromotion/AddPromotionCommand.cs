using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Promotions.Handler.Commands.AddPromotion
{
    public record AddPromotionCommand(string Title, string Description, bool HasImage, string? Link, string? Base64File) : IRequest<ResponseModel<bool>>;
}
