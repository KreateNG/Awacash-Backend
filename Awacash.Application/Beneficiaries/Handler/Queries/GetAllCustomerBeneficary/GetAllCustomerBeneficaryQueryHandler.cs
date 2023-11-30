using Awacash.Application.Beneficiaries.DTOs;
using Awacash.Application.Beneficiaries.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Beneficiaries.Handler.Queries.GetAllCustomerBeneficary
{
    public class GetAllCustomerBeneficaryQueryHandler : IRequestHandler<GetAllCustomerBeneficaryQuery, ResponseModel<List<BeneficiaryDTO>>>
    {
        private readonly IBeneficiaryService _beneficiaryService;

        public GetAllCustomerBeneficaryQueryHandler(IBeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }

        public async Task<ResponseModel<List<BeneficiaryDTO>>> Handle(GetAllCustomerBeneficaryQuery request, CancellationToken cancellationToken)
        {
            return await _beneficiaryService.GetAllCustomerBeneficariesAsyc();
        }
    }
}
