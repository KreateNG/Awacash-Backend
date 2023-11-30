using System;
using Awacash.Application.FeeConfigurations.DTOs;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.FeeConfigurations.Handler.Queries.GetFeeById
{
    public record GetFeeByIdQuery(string Id) : IRequest<ResponseModel<FeeConfigurationDto>>;
}

