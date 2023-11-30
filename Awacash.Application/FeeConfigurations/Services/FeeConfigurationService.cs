using AutoMapper;
using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Application.FeeConfigurations.DTOs;
using Awacash.Domain.Entities;
using Awacash.Domain.Enums;
using Awacash.Domain.Interfaces;
using Awacash.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.FeeConfigurations.Services
{
    public class FeeConfigurationService : IFeeConfigurationService
    {
        private readonly ILogger<FeeConfigurationService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;

        public FeeConfigurationService(ILogger<FeeConfigurationService> logger, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IMapper mapper, ICurrentUser currentUser)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
            _currentUser = currentUser;
        }
        public async Task<ResponseModel<bool>> CreateFeeConfigurationAsync(TransactionType? transactionType, decimal? upperBound, decimal? lowerBound, decimal? fee)
        {
            try
            {
                var feeConfig = new FeeConfiguration
                {
                    TransactionType = transactionType.Value,
                    UpperBound = upperBound.Value,
                    LowerBound = lowerBound.Value,
                    Fee = fee.Value,
                    CreatedBy = "sys",
                    CreatedDate = DateTime.UtcNow,
                    CreatedByIp = "::1"
                };

                _unitOfWork.FeeConfigurationRepository.Add(feeConfig);
                await _unitOfWork.Complete();
                return ResponseModel<bool>.Success(true);
            }
            catch (Exception ex)
            {

                return ResponseModel<bool>.Failure("error occure while creating fee configuration");
            }
        }

        public async Task<ResponseModel<List<FeeConfigurationDto>>> GetAllFeeConfigurationAsync()
        {
            try
            {
                var fee = await _unitOfWork.FeeConfigurationRepository.ListAllAsync();

                return ResponseModel<List<FeeConfigurationDto>>.Success(_mapper.Map<List<FeeConfigurationDto>>(fee.ToList()));
            }
            catch (Exception ex)
            {

                return ResponseModel<List<FeeConfigurationDto>>.Failure("error occured while fetching fee");
            }
        }

        public async Task<ResponseModel<FeeConfigurationDto>> GetFeeConfigurationByIdAsync(string id)
        {
            try
            {
                var fee = await _unitOfWork.FeeConfigurationRepository.GetByAsync(x => x.Id == id);
                if (fee is null)
                {
                    return ResponseModel<FeeConfigurationDto>.Failure("Fee configuration not found");
                };
                return ResponseModel<FeeConfigurationDto>.Success(_mapper.Map<FeeConfigurationDto>(fee));
            }
            catch (Exception ex)
            {

                return ResponseModel<FeeConfigurationDto>.Failure("error occured while fetching fee");
            }
        }
        public async Task<ResponseModel<decimal>> GetTransactionFeeAsync(decimal amount, TransactionType transactionType)
        {
            try
            {
                var fee = await _unitOfWork.FeeConfigurationRepository.GetByAsync(x => x.TransactionType == transactionType && amount >= x.LowerBound && amount <= x.UpperBound);
                if (fee is null)
                {
                    return ResponseModel<decimal>.Success(0);
                };
                return ResponseModel<decimal>.Success(fee.Fee);
            }
            catch (Exception ex)
            {

                return ResponseModel<decimal>.Failure("error occured while fetching fee");
            }
        }
        public async Task<ResponseModel<bool>> UpdateFeeConfigurationAsync(string id, TransactionType transactionType, decimal upperBound, decimal lowerBound, decimal fee)
        {
            try
            {
                var feeConfiguration = await _unitOfWork.FeeConfigurationRepository.GetByAsync(x => x.Id == id);
                if (feeConfiguration is null)
                {
                    return ResponseModel<bool>.Failure("Fee configuration not found");
                }
                feeConfiguration.TransactionType = transactionType;
                feeConfiguration.LowerBound = lowerBound;
                feeConfiguration.UpperBound = upperBound;
                feeConfiguration.Fee = fee;
                feeConfiguration.ModifiedBy = _currentUser.GetUserEmail();
                feeConfiguration.ModifiedDate = _dateTimeProvider.UtcNow;
                _unitOfWork.FeeConfigurationRepository.Update(feeConfiguration);
                await _unitOfWork.Complete();
                return ResponseModel<bool>.Success(true, "Fee configuration updated successfuly");
            }
            catch (Exception ex)
            {
                return ResponseModel<bool>.Failure("error occured while updating fee");
            }
        }
    }
}
