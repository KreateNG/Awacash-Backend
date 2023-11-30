using System;
using Awacash.Application.FeeConfigurations.DTOs;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.FeeConfigurations.Handler.Queries.GetAllFee
{
    public record GetAllFeesQuery() : IRequest<ResponseModel<List<FeeConfigurationDto>>>;
}

