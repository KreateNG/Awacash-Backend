using Awacash.Application.DisputeLogs.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.DisputeLogs.Handler.Commands.CreateDispuste
{
    public class CreateDisputeCommandHandler : IRequestHandler<CreateDisputeCommand, ResponseModel<bool>>
    {
        private readonly IDisputeLogService _disputeLogService;

        public CreateDisputeCommandHandler(IDisputeLogService disputeLogService)
        {
            _disputeLogService = disputeLogService;
        }

        public async Task<ResponseModel<bool>> Handle(CreateDisputeCommand request, CancellationToken cancellationToken)
        {
            return await _disputeLogService.CreateDisputeLogAsync(request.AccountNumber, request.Email, request.PhoneNumber, request.Amount, request.TransactionDate, request.Comment);
        }
    }
}
