using Awacash.Application.Savings.DTOs;
using Awacash.Application.Savings.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Savings.Handler.Commands.CreateSaving
{
    public class CreateSavingCommandHandler : IRequestHandler<CreateSavingCommand, ResponseModel<SavingDTO>>
    {
        private readonly ISavingService _savingService;

        public CreateSavingCommandHandler(ISavingService savingService)
        {
            _savingService = savingService;
        }

        public async Task<ResponseModel<SavingDTO>> Handle(CreateSavingCommand request, CancellationToken cancellationToken)
        {
            return await _savingService.CreateSavingAsync(request.Reason, request.TargetAmount.Value, request.DeductionFrequency.Value, request.SavingConfigId, request.SavingsDuration.Value, request.DeductionAccount);
        }
    }
}
