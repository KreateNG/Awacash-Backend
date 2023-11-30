using AutoMapper;
using Awacash.Application.CardRequestConfigurations.DTOs;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Domain.Entities;
using Awacash.Domain.Enums;
using Awacash.Domain.Interfaces;
using Awacash.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.CardRequestConfigurations.Services
{
    public class CardRequestConfigurationService : ICardRequestConfigurationService
    {
        private readonly ILogger<CardRequestConfigurationService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;
        public CardRequestConfigurationService(ILogger<CardRequestConfigurationService> logger, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
        }

        public async Task<ResponseModel<bool>> CreateCardRequestConfigurationAsync(string IssuerName, decimal price, CardType cardType)
        {
            try
            {
                var cardRequestConfig = await _unitOfWork.CardRequestConfigurationtRepository.GetByAsync(x => x.CardType == cardType);
                if (cardRequestConfig is not null)
                {
                    return ResponseModel<bool>.Failure($"Card type already exist");
                }
                var newCardConfig = new CardRequestConfiguration
                {
                    IssuerName = IssuerName,
                    CardType = cardType,
                    Price = price,
                    CreatedBy = "sys",
                    CreatedDate = _dateTimeProvider.UtcNow,
                    CreatedByIp = "::1"
                };
                _unitOfWork.CardRequestConfigurationtRepository.Add(newCardConfig);
                await _unitOfWork.Complete();
                return ResponseModel<bool>.Success(true);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message, nameof(CreateCardRequestConfigurationAsync));
                return ResponseModel<bool>.Failure($"error occured while creating card request congiguration");
            }
        }

        public async Task<ResponseModel<List<CardRequestConfigurationDTO>>> GetAllCardRequestConfigurationAsync()
        {
            try
            {
                var cardRequestConfig = await _unitOfWork.CardRequestConfigurationtRepository.ListAllAsync();
                if (cardRequestConfig is null)
                {
                    return ResponseModel<List<CardRequestConfigurationDTO>>.Failure($"Card request configuration not found");
                }

                return ResponseModel<List<CardRequestConfigurationDTO>>.Success(_mapper.Map<List<CardRequestConfigurationDTO>>(cardRequestConfig));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message, nameof(CreateCardRequestConfigurationAsync));
                return ResponseModel<List<CardRequestConfigurationDTO>>.Failure($"error occured while fetching card request congiguration by id");
            }
        }

        public async Task<ResponseModel<CardRequestConfigurationDTO>> GetCardRequestConfigurationByIdAsync(string id)
        {
            try
            {
                var cardRequestConfig = await _unitOfWork.CardRequestConfigurationtRepository.GetByAsync(x => x.Id == id);
                if (cardRequestConfig is null)
                {
                    return ResponseModel<CardRequestConfigurationDTO>.Failure($"Card request configuration not found");
                }

                return ResponseModel<CardRequestConfigurationDTO>.Success(_mapper.Map<CardRequestConfigurationDTO>(cardRequestConfig));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message, nameof(CreateCardRequestConfigurationAsync));
                return ResponseModel<CardRequestConfigurationDTO>.Failure($"error occured while fetching card request congiguration by id");
            }
        }

        public async Task<ResponseModel<bool>> UpdateCardRequestConfigurationAsync(string id, string IssuerName, decimal price)
        {
            try
            {
                var cardRequestConfig = await _unitOfWork.CardRequestConfigurationtRepository.GetByAsync(x => x.Id == id);
                if (cardRequestConfig is null)
                {
                    return ResponseModel<bool>.Failure($"Card request configuration not found");
                }
                cardRequestConfig.IssuerName = IssuerName;
                cardRequestConfig.Price = price;
                cardRequestConfig.ModifiedBy = "sys";
                cardRequestConfig.ModifiedDate = _dateTimeProvider.UtcNow;
                cardRequestConfig.ModifiedByIp = "::1";
                
                _unitOfWork.CardRequestConfigurationtRepository.Update(cardRequestConfig);
                await _unitOfWork.Complete();
                return ResponseModel<bool>.Success(true);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message, nameof(CreateCardRequestConfigurationAsync));
                return ResponseModel<bool>.Failure($"error occured while updating card request congiguration");
            }
        }
    }
}
