using System;
using Awacash.Application.DashBoard.DTOs;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;

namespace Awacash.Application.DashBoard.Services
{
    public interface IDashBoardService
    {
        Task<ResponseModel<DashBoardDto>> GetDashBoardDataAsync();
    }
}

