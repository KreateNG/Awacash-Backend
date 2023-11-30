using Awacash.Application.CardRequestConfigurations.DTOs;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.CardRequestConfigurations.Handler.Queries.GetAllCardRequestConfiguration
{
    public record GetAllCardRequestConfigurationQuery():IRequest<ResponseModel<List<CardRequestConfigurationDTO>>>;
}
