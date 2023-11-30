using Awacash.Application.Savings.DTOs;
using Awacash.Application.Savings.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Savings.Handler.Queries.GetAllSavings
{
    public class GetAllSavingsQueryHandler : IRequestHandler<GetAllSavingsQuery, ResponseModel<List<SavingDTO>>>
    {
        private readonly ISavingService _savingService;

        public GetAllSavingsQueryHandler(ISavingService savingService)
        {
            _savingService = savingService;
        }

        public async Task<ResponseModel<List<SavingDTO>>> Handle(GetAllSavingsQuery request, CancellationToken cancellationToken)
        {
            return await _savingService.GetAllSavingsAsync();
        }
    }
}
