using Awacash.Domain.Enums;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.CardRequestConfigurations.Handler.Commands.CreateCardRequestConfiguration
{
    public record CreateCardRequestConfigurationCommand(string IssuerName, decimal Price, CardType CardType):IRequest<ResponseModel<bool>>;
}
