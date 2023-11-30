using System;
using AutoMapper;
using Awacash.Application.DashBoard.DTOs;
using Awacash.Application.Transactions.DTOs;
using Awacash.Domain.Common.Constants;
using Awacash.Domain.Enums;
using Awacash.Domain.Interfaces;
using Awacash.Shared;
using Microsoft.Extensions.Logging;

namespace Awacash.Application.DashBoard.Services
{
    public class DashBoardService : IDashBoardService
    {
        private readonly ILogger<DashBoardService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DashBoardService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DashBoardService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseModel<DashBoardDto>> GetDashBoardDataAsync()
        {
            try
            {
                var transactionsTask = await _unitOfWork.TransactionRepository.ListAllAsync();

                var totalTransactionsVolumeTask = await _unitOfWork.TransactionRepository.FindSum(x => x.Status.ToLower() == TransactionStatus.SUCCESSFUL.ToLower(), x => x.Amount);

                var totalTransactionsTask = await _unitOfWork.TransactionRepository.CountAsync(x => x.Status.ToLower() == TransactionStatus.SUCCESSFUL.ToLower());

                var customersTask = await _unitOfWork.CustomerRepository.CountAsync();
                var cardRequestsTask = await _unitOfWork.CardRequestRepository.CountAsync();

                //await Task.WhenAll(transactionsTask, totalTransactionsVolumeTask, totalTransactionsTask, customersTask, cardRequestsTask);
                var dashBoardDto = new DashBoardDto();
                dashBoardDto.TotalCardRequest = cardRequestsTask;
                dashBoardDto.TotalCustomers = customersTask;
                dashBoardDto.TotalTransactions = totalTransactionsTask;
                dashBoardDto.TotalTransactionVolume = totalTransactionsVolumeTask;
                dashBoardDto.Transactions = _mapper.Map<List<TransactionDTO>>(transactionsTask.OrderByDescending(x => x.CreatedDate).Take(20).ToList());

                return ResponseModel<DashBoardDto>.Success(dashBoardDto);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<DashBoardDto>.Failure("Error occure please try again later");
            }
        }
    }
}

