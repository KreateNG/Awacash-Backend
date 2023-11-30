using Awacash.Domain.Interfaces;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Wema.Services
{
    public class WemaService: IWemaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WemaService> _logger;

        public WemaService(IUnitOfWork unitOfWork, ILogger<WemaService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseModel<NameEnquiryResponse>> WemaAccointLookUp(string accountNumber)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.AccountNumber == accountNumber && x.IsDeleted == false);
                if (customer is null)
                {
                    return ResponseModel<NameEnquiryResponse>.Failure("Customer not found");
                }
                return ResponseModel<NameEnquiryResponse>.Success(new NameEnquiryResponse($"Awacash/{customer.FirstName} {customer.LastName}"));
            }
            catch (Exception ex)
            {

                _logger.LogCritical(ex.Message);
                return ResponseModel<NameEnquiryResponse>.Failure("Error occure please try again later");
            }
        }
    }
}
