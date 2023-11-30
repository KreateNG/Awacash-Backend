using System;
using Awacash.Application.DashBoard.DTOs;
using Awacash.Application.DashBoard.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.DashBoard.Handler.Queries
{
    public class GetDashBoardDataQueryHandler : IRequestHandler<GetDashBoardDataQuery, ResponseModel<DashBoardDto>>
    {
        private readonly IDashBoardService _dashBoardService;
        public GetDashBoardDataQueryHandler(IDashBoardService dashBoardService)
        {
            _dashBoardService = dashBoardService;
        }

        public async Task<ResponseModel<DashBoardDto>> Handle(GetDashBoardDataQuery request, CancellationToken cancellationToken)
        {
            return await _dashBoardService.GetDashBoardDataAsync();
        }
    }
}

