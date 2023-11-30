using Awacash.Application.Beneficiaries.DTOs;
using Awacash.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Beneficiaries.Services
{
    public interface IBeneficiaryService
    {
        Task<ResponseModel<List<BeneficiaryDTO>>> GetAllCustomerBeneficariesAsyc();
        Task<ResponseModel<BeneficiaryDTO>> GetBeneficaryByIdAsyc(string Id);
    }
}
