using Awacash.Application.Savings.DTOs;
using Awacash.Application.Savings.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Savings.Handler.Queries.GetAllSavingsById
{
    public class GetAllSavingsByIdQueryHandler : IRequestHandler<GetAllSavingsByIdQuery, ResponseModel<List<SavingDTO>>>
    {
        private readonly ISavingService _savingService;

        public GetAllSavingsByIdQueryHandler(ISavingService savingService)
        {
            _savingService = savingService;
        }

        public async Task<ResponseModel<List<SavingDTO>>> Handle(GetAllSavingsByIdQuery request, CancellationToken cancellationToken)
        {
            return await _savingService.GetAllSavingsByIdAsync();
        }
    }
}
