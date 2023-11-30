using Awacash.Application.Savings.DTOs;
using Awacash.Application.Savings.Handler.Queries.GetAllSavingsById;
using Awacash.Application.Savings.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Savings.Handler.Queries.GetSavingById
{
    public class GetSavingByIdQueryHandler : IRequestHandler<GetSavingByIdQuery, ResponseModel<SavingDTO>>
    {
        private readonly ISavingService _savingService;

        public GetSavingByIdQueryHandler(ISavingService savingService)
        {
            _savingService = savingService;
        }

        public async Task<ResponseModel<SavingDTO>> Handle(GetSavingByIdQuery request, CancellationToken cancellationToken)
        {
            return await _savingService.GetSavingByIdAsync(request.Id);
        }
    }
}
