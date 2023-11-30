using System;
using Awacash.Application.CardRequests.DTOs;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.CardRequests.Handler.Queries.GetById
{
    public record GetCardRequestByIdQuery(string Id) : IRequest<ResponseModel<CardRequestDTO>>;
}

