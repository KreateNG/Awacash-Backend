using System;
using Awacash.Application.DashBoard.DTOs;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.DashBoard.Handler.Queries
{
    public record GetDashBoardDataQuery : IRequest<ResponseModel<DashBoardDto>>;
}

