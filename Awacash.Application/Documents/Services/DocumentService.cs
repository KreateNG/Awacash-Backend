using System;
using System.Text;
using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Application.Customers.Services;
using Awacash.Domain.Entities;
using Awacash.Domain.Interfaces;
using Awacash.Domain.Settings;
using Awacash.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static System.Net.Mime.MediaTypeNames;

namespace Awacash.Application.Documents.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ILogger<DocumentService> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IHostEnvironment _hostingEnvironment;
        private readonly ICryptoService _cryptoService;
        private readonly AppSettings _appSettings;
        public DocumentService(ILogger<DocumentService> logger, ICurrentUser currentUser, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IHostEnvironment hostingEnvironment, ICryptoService cryptoService, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _hostingEnvironment = hostingEnvironment;
            _cryptoService = cryptoService;
            _appSettings = appSettings.Value;
        }
        public async Task<ResponseModel<bool>> CreateDocuent(string? idBase64, string? utilityBase64, string? IDNumber, FileType? FileType)
        {
            try
            {
                var userId = _currentUser.GetCustomerId();
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == userId && x.IsDeleted == false);
                if (customer is null)
                {
                    return ResponseModel<bool>.Failure("Customer not found");
                }

                var (valid, error, ext) = ValidateImage(idBase64);
                if (!valid)
                {
                    return ResponseModel<bool>.Failure(error);
                }

                var fileName = $"Awacash_{_dateTimeProvider.UtcNow.ToString("yyyyMMddHHmmss")}_{customer.FullName.Replace(" ", "_")}_{_cryptoService.GetNextInt64().ToString().Substring(0, 4)}.{ext}";
                var target = System.IO.Path.Combine(_appSettings.SystemPath + _appSettings.ProfilePath, fileName);
                await File.WriteAllBytesAsync(target, Convert.FromBase64String(idBase64));
                var imageUrl = $"{_appSettings.DomainName}{_appSettings.ProfilePath}/{fileName}";

                _unitOfWork.DocumentRepository.Add(new Document()
                {
                    Customer = customer,
                    IDNumber = IDNumber,
                    ResourceUrl = imageUrl,
                    Status = StatusEnum.Pending,
                    VerificationTrial = 0,
                    FileType = FileType,
                    CreatedBy = customer.Email,
                    CreatedByIp = "::1",
                    CreatedDate = _dateTimeProvider.UtcNow
                });


                var (valid2, error2, ext2) = ValidateImage(utilityBase64);
                if (!valid2)
                {
                    return ResponseModel<bool>.Failure(error2);
                }

                var utilityFileName = $"Awacash_{_dateTimeProvider.UtcNow.ToString("yyyyMMddHHmmss")}_{customer.FullName.Replace(" ", "_")}_{_cryptoService.GetNextInt64().ToString().Substring(0, 4)}.{ext}";
                var target2 = System.IO.Path.Combine(_appSettings.SystemPath + _appSettings.ProfilePath, utilityFileName);
                await File.WriteAllBytesAsync(target, Convert.FromBase64String(utilityBase64));
                var imageUrl2 = $"{_appSettings.DomainName}{_appSettings.ProfilePath}/{utilityFileName}";

                _unitOfWork.DocumentRepository.Add(new Document()
                {
                    Customer = customer,
                    ResourceUrl = imageUrl2,
                    Status = StatusEnum.Pending,
                    VerificationTrial = 0,
                    CreatedBy = customer.Email,
                    CreatedByIp = "::1",
                    CreatedDate = _dateTimeProvider.UtcNow
                });

                customer.IsIdUploaded = true;
                _unitOfWork.CustomerRepository.Update(customer);


                await _unitOfWork.Complete();

                return ResponseModel<bool>.Success(true);

                //string baseDirectory = _hostingEnvironment.ContentRootPath;
                //string tmplFolder = Path.Combine(Directory.GetCurrentDirectory(), "Assets/EmailTemplates");
                //var fileName = $"Awacash{_dateTimeProvider.UtcNow.ToString("yyyyMMddHHmmss")}_{customer.FullName.Replace(" ", "_")}_{_cryptoService.GetNextInt64().ToString().Substring(0, 4)}.{ext}";

                //string filePath = Path.Combine(tmplFolder, fileName);
                //await File.WriteAllBytesAsync(filePath, Convert.FromBase64String(passwordBase64));

            }
            catch (Exception ex)
            {

            }
            throw new NotImplementedException();
        }



        private Tuple<bool, string, string> ValidateImage(string base64String, bool isPassport = false)
        {
            bool isValid = false;
            string error = string.Empty;
            string ext = string.Empty;

            if (!ValidateDocumentSize(base64String))
            {
                error = $"File size should not be more than 5MB";
                return new Tuple<bool, string, string>(isValid, error, ext);
            }

            if (!ValidateDocumentType(base64String, out ext))
            {
                error = $"Invalid file type.";
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

