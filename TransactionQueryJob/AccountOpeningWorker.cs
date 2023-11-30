using System;
using Awacash.Domain.Entities;
using Awacash.Domain.Enums;
using Awacash.Domain.Helpers;
using Awacash.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using TransactionQueryJob.Data;
using TransactionQueryJob.Interfaces;

namespace TransactionQueryJob
{
    public class AccountOpeningWorker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly AppDbContext _dbContext;
        private readonly ICommunicationService _communicationService;
        private readonly IBankOneAccountService _bankOneAccountService;
        public AccountOpeningWorker(ILogger<Worker> logger, AppDbContext dbContext, ICommunicationService communicationService, IBankOneAccountService bankOneAccountService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _communicationService = communicationService;
            _bankOneAccountService = bankOneAccountService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await ProcessAccountOpeningRequest();
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private async Task ProcessAccountOpeningRequest()
        {
            try
            {
                _logger.LogInformation("Calling get all customer that are have verified bvn without Account: {time}", DateTimeOffset.Now);
                var customers = await _dbContext.Customers.Where(x => string.IsNullOrWhiteSpace(x.AccountId) && x.IsBvnConfirmed).ToListAsync();
                if (customers != null)
                {
                    foreach (var customer in customers)
                    {
                        _logger.LogInformation("Calling bvn info data at: {time}", DateTimeOffset.Now);
                        var bvnInfo = await _dbContext.BvnInfo.Where(x => x.FirstName.Equals(customer.FirstName, StringComparison.CurrentCultureIgnoreCase) && x.Surname.Equals(customer.LastName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefaultAsync();
                        if (bvnInfo != null)
                        {
                            _logger.LogInformation($"Get Customer with account number {customer.AccountNumber} wallet at: {DateTimeOffset.Now}");

                            var nunBanAccountRes = await _bankOneAccountService.AccountOpening(bvnInfo.FirstName, bvnInfo.Surname, bvnInfo.MiddleName, "", bvnInfo.PhoneNumber1, bvnInfo.Gender.ToLower(), "", bvnInfo.DateOfBirth.ToString(), "", bvnInfo.Nin, customer.Email, "", "", "", "");

                            if (nunBanAccountRes == null || !nunBanAccountRes.IsSuccessful || nunBanAccountRes.Data == null)
                            {
                                //customer.AccountId = "P";

                            }
                            else
                            {
                                customer.AccountId = nunBanAccountRes.Data.CustomerID;
                            }
                            customer.ModifiedBy = "Admin proccess";
                            customer.ModifiedDate = DateTime.UtcNow;
                            customer.ModifiedByIp = "::1";
                            _dbContext.Customers.Update(customer);

                            await _dbContext.SaveChangesAsync();

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "{Message}", ex.Message);
                Environment.Exit(1);
            }

        }
    }
}

