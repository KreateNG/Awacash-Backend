using AutoMapper;
using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Application.Promotions.DTOs;
using Awacash.Application.Savings.DTOs;
using Awacash.Application.Savings.Services;
using Awacash.Domain.Entities;
using Awacash.Domain.Interfaces;
using Awacash.Domain.Settings;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Awacash.Domain.Common.Errors.Errors;
using static System.Net.Mime.MediaTypeNames;

namespace Awacash.Application.Promotions.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly ILogger<PromotionService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;
        private readonly ICryptoService _cryptoService;
        private readonly AppSettings _appSettings;

        public PromotionService(IUnitOfWork unitOfWork, ILogger<PromotionService> logger, ICurrentUser currentUser, IDateTimeProvider dateTimeProvider, IMapper mapper, ICryptoService cryptoService, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _currentUser = currentUser;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
            _cryptoService = cryptoService;
            _appSettings = appSettings.Value;
        }

        public async Task<ResponseModel<bool>> CreatePromotion(string title, string description, bool hasImage, string? Link,
        string? base64File = null)
        {
            try
            {
                var promotion = new Promotion()
                {
                    Title = title,
                    Link = Link,
                    Description = description,
                    HasImage = hasImage,
                    CreatedBy = "sys",
                    CreatedByIp = "::1",
                    CreatedDate = DateTime.Now,
                };

                if (hasImage && !string.IsNullOrWhiteSpace(base64File))
                {
                    var (valid, error, ext) = ValidateImage(base64File);
                    if (!valid)
                    {
                        return ResponseModel<bool>.Failure(error);
                    }
                    // TODO upload file and return url
                    var fileName = $"Berachah_{_dateTimeProvider.UtcNow.ToString("yyyyMMddHHmmss")}_{_cryptoService.GetNextInt64().ToString().Substring(0, 4)}.{ext}";
                    var target = System.IO.Path.Combine(_appSettings.SystemPath + _appSettings.PromotionPath, fileName);
                    await File.WriteAllBytesAsync(target, Convert.FromBase64String(base64File));
                    var imageUrl = $"{_appSettings.DomainName}{_appSettings.PromotionPath}/{fileName}";
                    promotion.ImageUrl = imageUrl;
                }
                _unitOfWork.PromotionRepository.Add(promotion);
                await _unitOfWork.Complete();

                return ResponseModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Erorr on creating promotion ==> {ex.Message}", nameof(CreatePromotion));
                return ResponseModel<bool>.Failure("");
            }
        }

        public async Task<ResponseModel<List<PromotionDTO>>> GetAllPromotion()
        {
            try
            {
                var promotions = await _unitOfWork.PromotionRepository.ListAllAsync();
                return ResponseModel<List<PromotionDTO>>.Success(_mapper.Map<List<PromotionDTO>>(promotions));
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while getting promotion: {ex.Message}", nameof(GetAllPromotion));
                return ResponseModel<List<PromotionDTO>>.Failure("Exception error");
            }
        }

        public Task<ResponseModel<PagedResult<PromotionDTO>>> GetPagedPromotion()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<PromotionDTO>> GetPromotionById(string id)
        {
            try
            {
                var promotion = await _unitOfWork.PromotionRepository.GetByAsync(x => x.Id == id);
                return ResponseModel<PromotionDTO>.Success(_mapper.Map<PromotionDTO>(promotion));
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while getting promotion: {ex.Message}", nameof(GetAllPromotion));
                return ResponseModel<PromotionDTO>.Failure("Exception error");
            }
        }

        public Task<ResponseModel<bool>> UpdatePromotion(string title, string description, bool hasImage)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<bool>> UpdatePromotionImage(string id, string? base64File = null)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<bool>> UpdatePromotionStatus(string id)
        {
            throw new NotImplementedException();
        }

        private Tuple<bool, string, string> ValidateImage(string base64String, bool isPassport = false)
        {
            bool isValid = false;
            string error = string.Empty;
            string ext = string.Empty;

            if (!ValidateDocumentSize(base64String))
            {
                error = $"image size should not be more than 5MB";
                return new Tuple<bool, string, string>(isValid, error, ext);
            }

            if (!ValidateDocumentType(base64String, out ext))
            {
                error = $"Invalid image file.";
                return new Tuple<bool, string, string>(isValid, error, ext);
            }
            isValid = true;
            return new Tuple<bool, string, string>(isValid, error, ext);

        }

        private bool ValidateDocumentSize(string base64String)
        {
            bool isValid = false;

            var stringLength = base64String.Length - "data:image/png;base64,".Length;
            decimal actualLenght = stringLength / 4;
            var sizeInBytes = Math.Ceiling(actualLenght) * 3;
            var sizeInKb = sizeInBytes / 1000;

            if (sizeInKb < (1024 * 5))
            {
                isValid = true;
            }

            return isValid;
        }

        private bool ValidateDocumentType(string base64String, out string extention)
        {
            bool isValid = false;
            extention = string.Empty;
            var data = base64String.Substring(0, 5);
            if (!string.IsNullOrWhiteSpace(data))
            {
                if (data.ToUpper().Equals("IVBOR") || data.ToUpper().Equals("/9J/4".ToUpper()))
                {
                    extention = data.ToUpper().Equals("IVBOR") ? "png" : "jpg";
                    isValid = true;
                }
                //else
                //{
                //    if (data.ToUpper().Equals("IVBOR") || data.ToUpper().Equals("/9J/4".ToUpper()) || data.ToUpper().Equals("JVBER".ToUpper()))
                //    {
                //        isValid = true;
                //    }
                //}

            }

            return isValid;
        }
    }
}
