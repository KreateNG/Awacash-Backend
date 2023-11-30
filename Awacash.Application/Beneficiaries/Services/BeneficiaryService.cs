using AutoMapper;
using Awacash.Application.Beneficiaries.DTOs;
using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Domain.Interfaces;
//using Awacash.Domain.Models.BillsPayment;
using Awacash.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Beneficiaries.Services
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly ILogger<BeneficiaryService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;

        public BeneficiaryService(ILogger<BeneficiaryService> logger, IUnitOfWork unitOfWork, IMapper mapper, ICurrentUser currentUser)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }


        public async Task<ResponseModel<List<BeneficiaryDTO>>> GetAllCustomerBeneficariesAsyc()
        {
            try
            {
                var customerId = _currentUser.GetCustomerId();
                var beneficary = await _unitOfWork.BeneficiaryRepository.ListAsync(x => x.CustomerId == customerId);
                return ResponseModel<List<BeneficiaryDTO>>.Success(_mapper.Map<List<BeneficiaryDTO>>(beneficary));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting customers beneficary: {ex.Message}", nameof(GetAllCustomerBeneficariesAsyc));
                return ResponseModel<List<BeneficiaryDTO>>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<BeneficiaryDTO>> GetBeneficaryByIdAsyc(string Id)
        {
            try
            {
                var beneficary = await _unitOfWork.BeneficiaryRepository.GetByAsync(x => x.Id == Id);
                return ResponseModel<BeneficiaryDTO>.Success(_mapper.Map<BeneficiaryDTO>(beneficary));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting beneficary by id: {ex.Message}", nameof(GetBeneficaryByIdAsyc));
                return ResponseModel<BeneficiaryDTO>.Failure("Exception error");
            }
        }
    }
}
