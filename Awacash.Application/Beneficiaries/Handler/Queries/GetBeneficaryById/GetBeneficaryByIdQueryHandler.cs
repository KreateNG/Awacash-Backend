using Awacash.Application.Beneficiaries.DTOs;
using Awacash.Application.Beneficiaries.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Beneficiaries.Handler.Queries.GetBeneficaryById
{
    public class GetBeneficaryByIdQueryHandler : IRequestHandler<GetBeneficaryByIdQuery, ResponseModel<BeneficiaryDTO>>
    {
        private readonly IBeneficiaryService _beneficiaryService;

        public GetBeneficaryByIdQueryHandler(IBeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }

        public async Task<ResponseModel<BeneficiaryDTO>> Handle(GetBeneficaryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _beneficiaryService.GetBeneficaryByIdAsyc(request.Id);
        }
    }
}
