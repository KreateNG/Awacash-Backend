using System;
using AutoMapper;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Domain.Entities;
using Awacash.Domain.Interfaces;
using Awacash.Shared;
using AwaCash.Application.Common.Exceptions;
using AwaCash.Application.Common.Model;
using Microsoft.Extensions.Logging;

namespace AwaCash.Application.Common.Interfaces.Services
{
    public class OtpService : IOtpService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<OtpService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICryptoService _cryptoService;

        public OtpService(IMapper mapper, ILogger<OtpService> logger, IUnitOfWork uow, ICryptoService cryptoService)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = uow;
            _cryptoService = cryptoService;
        }

        public async Task<ResponseModel<OtpResponseDTO>> CreateAsync(string resourceId, string resourceName)
        {
            var context = _unitOfWork.TokenRepository;
            var code = _cryptoService.GenerateNumericKey(6);
            var entity = new Token
            {
                Code = code,
                CreatedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddHours(24),
                ResourceId = resourceId,
                ResourceName = resourceName,
            };
            context.Add(entity);

            await _unitOfWork.Complete();

            return ResponseModel<OtpResponseDTO>.Success(_mapper.Map<OtpResponseDTO>(entity));
        }


        public async Task<ResponseModel<OtpResponseDTO>> Reset(string id)
        {
            var context = _unitOfWork.TokenRepository;

            var entity = await context.GetByIdAsync(id);

            if (entity == null)
            {
                throw new ResourceNotFoundException("security resource not found");
            }

            if (entity.ExpiryDate < DateTime.UtcNow)
            {
                entity.ExpiryDate = DateTime.UtcNow.AddHours(24);
                entity.Code = _cryptoService.GenerateNumericKey(6);
                entity.UpdateLastModifiedDate();
            }
            await _unitOfWork.Complete();

            return ResponseModel<OtpResponseDTO>.Success(_mapper.Map<OtpResponseDTO>(entity));

        }



        public async Task<ResponseModel<OtpResponseDTO>> Use(string id)
        {
            var context = _unitOfWork.TokenRepository;

            var entity = await context.GetByIdAsync(id);

            if (entity == null)
            {
                throw new ResourceNotFoundException("security resource not found");
            }
            if (entity.IsUsed)
            {
                return ResponseModel<OtpResponseDTO>.Success(_mapper.Map<OtpResponseDTO>(entity));

            }
            entity.IsUsed = true;
            entity.UpdateLastModifiedDate();
            await _unitOfWork.Complete();

            return ResponseModel<OtpResponseDTO>.Success(_mapper.Map<OtpResponseDTO>(entity));


        }

        public Task<ResponseModel<string>> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }


        public async Task<ResponseModel<OtpResponseDTO>> GetSingleAsync(string id)
        {
            var context = _unitOfWork.TokenRepository;

            var entity = await context.GetByIdAsync(id);

            if (entity == null)
            {
                throw new ResourceNotFoundException("security resource not found");
            }

            return ResponseModel<OtpResponseDTO>.Success(_mapper.Map<OtpResponseDTO>(entity));


        }
    }
}

