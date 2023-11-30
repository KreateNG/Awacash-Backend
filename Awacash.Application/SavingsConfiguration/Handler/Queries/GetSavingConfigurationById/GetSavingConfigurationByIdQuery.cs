using Awacash.Application.SavingsConfiguration.DTOs;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.SavingsConfiguration.Handler.Queries.GetSavingConfigurationById
{
    public record GetSavingConfigurationByIdQuery(string Id):IRequest<ResponseModel<SavingConfigurationDTO>>;
}
