using System;
using AutoMapper;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Application.Users.Specifications;
using Awacash.Application.Wallets.DTOs;
using Awacash.Application.Wallets.FilterModels;
using Awacash.Application.Wallets.Specifications;
using Awacash.Domain.Common.Constants;
using Awacash.Domain.Extentions;
using Awacash.Domain.Interfaces;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using Microsoft.Extensions.Logging;

namespace Awacash.Application.Wallets.Services
{
    public class WalletService : IWalletService
    {
        private readonly ILogger<WalletService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        public WalletService(IUnitOfWork unitOfWork, ILogger<WalletService> logger, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }
        public async Task<ResponseModel<List<WalletDTO>>> GetAllWalletAsync()
        {
            try
            {
                var wallets = await _unitOfWork.WalletRepository.ListAllAsync();
                return ResponseModel<List<WalletDTO>>.Success(_mapper.Map<List<WalletDTO>>(wallets?.ToList()));
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while fetching wallets: {ex.Message}", nameof(GetAllWalletAsync));
                return ResponseModel<List<WalletDTO>>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<PagedResult<WalletDTO>>> GetPaginatedWalletAsync(WalletFilterModel walletFilterModel)
        {
            try
            {
                var walletSpecification = new WalletFilterSpecification(firstname: walletFilterModel.FirstName, lastname: walletFilterModel.LastName, phonenumber: walletFilterModel.PhoneNumber, status: walletFilterModel.Status);
                var wallets = await _unitOfWork.WalletRepository.ListAsync(walletFilterModel.PageIndex, walletFilterModel.PageSize, walletSpecification);

                return ResponseModel<PagedResult<WalletDTO>>.Success(_mapper.Map<PagedResult<WalletDTO>>(wallets));

            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while fetching paginated wallets: {ex.Message}", nameof(GetPaginatedWalletAsync));
                return ResponseModel<PagedResult<WalletDTO>>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<WalletDTO>> GetWalletByIdAsync(string Id)
        {
            try
            {
                var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(Id);

                return ResponseModel<WalletDTO>.Success(_mapper.Map<WalletDTO>(wallet));
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while getting wallet: {ex.Message}", nameof(GetWalletByIdAsync));
                return ResponseModel<WalletDTO>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<bool>> UpdateWalletStattusAsync(string Id)
        {
            try
            {
                var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(Id);
                if (wallet is null)
                {
                    return ResponseModel<bool>.Failure("wallet not found");
                }
                wallet.Status = wallet.Status == WalletStatus.ACTIVE ? WalletStatus.INACTIVE : WalletStatus.ACTIVE;
                wallet.ModifiedBy = "";
                wallet.ModifiedByIp = "::1";
                wallet.ModifiedDate = _dateTimeProvider.UtcNow;
                wallet.CheckSum = wallet.GetCheckSum();
                _unitOfWork.WalletRepository.Update(wallet);
                await _unitOfWork.Complete();

                return ResponseModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while updatting wallet status: {ex.Message}", nameof(UpdateWalletStattusAsync));
                return ResponseModel<bool>.Failure("Exception error");
            }
        }
    }
}

