using System;
using Awacash.Application.CardRequests.DTOs;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.CardRequests.Handler.Queries.GetAll
{
    public record GetAllCardRequestQuery : IRequest<ResponseModel<List<CardRequestDTO>>>;
}

