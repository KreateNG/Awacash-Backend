using System;
using Awacash.Application.CardRequests.DTOs;
using Awacash.Application.CardRequests.FilterModels;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;

namespace Awacash.Application.CardRequests.Handler.Queries.GetPagedList
{
    public class GetPaginatedCardRequestQuery : CardRequestFilterModel, IRequest<ResponseModel<PagedResult<CardRequestDTO>>>
    {

    }
}

