using Awacash.Domain.Entities;
using Awacash.Domain.Enums;
using Awacash.Domain.Extentions;
using Awacash.Domain.Helpers;
using Awacash.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using TransactionQueryJob.Data;
using TransactionQueryJob.Interfaces;

namespace TransactionQueryJob
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly AppDbContext _dbContext;
        private readonly ICommunicationService _communicationService;
        public Worker(ILogger<Worker> logger, AppDbContext dbContext, ICommunicationService communicationService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _communicationService = communicationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await ProcessRequest();
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }


        private async Task ProcessRequest()
        {
            try
            {
                _logger.LogInformation("Calling Account Transaction data at: {time}", DateTimeOffset.Now);
                var trxNotoffication = await _dbContext.AccountTransactions.Where(x => x.IsVerify == false).ToListAsync();
                if (trxNotoffication != null)
                {
                    var emailTemplate = await _dbContext.EmailTemplate.Where(x => x.EmailType == EmailType.Transaction).FirstOrDefaultAsync(); ;


                    foreach (var trx in trxNotoffication)
                    {
                        _logger.LogInformation("Calling Customer data at: {time}", DateTimeOffset.Now);
                        var customer = await _dbContext.Customers.SingleOrDefaultAsync(x => x.AccountNumber == trx.Craccount);
                        if (customer != null)
                        {
                            _logger.LogInformation($"Get Customer with account number {customer.AccountNumber} wallet at: {DateTimeOffset.Now}");
                            var wallet = await _dbContext.Wallets.SingleOrDefaultAsync(x => x.CustomerId == customer.Id);
                            decimal.TryParse(trx.Amount, out decimal amount);
                            if (wallet != null)
                            {
                                wallet.Balance += amount;
                                wallet.ModifiedBy = "SYS";
                                wallet.ModifiedDate = DateTime.UtcNow;

                                wallet.CheckSum = wallet.GetCheckSum();

                                _dbContext.Wallets.Update(wallet);

                                trx.IsVerify = true;
                                _dbContext.AccountTransactions.Update(trx);

                                var transaction = new Transaction()
                                {
                                    Amount = amount,
                                    CreditAccountName = trx.Craccountname,
                                    CreditAccountNumber = trx.Craccount,
                                    DebitAccountName = trx.Originatorname,
                                    DebitAccountNumber = trx.Originatoraccountnumber,
                                    DestinationBankCode = trx.Bankcode,
                                    DestinationBankName = trx.Bankname,
                                    CreatedBy = "Sys",
                                    CreatedDate = DateTime.UtcNow,
                                    CreatedByIp = "::1",
                                    Narration = trx.Narration,
                                    CustomerId = customer.Id,
                                    Status = "Success",
                                    TransactionReference = trx.TransactionReference,
                                    PaymentReference = trx.Paymentreference,
                                    SessionId = trx.Sessionid,
                                    Currency = "NGN",
                                    TransactionType = TransactionType.InterBankTransfer,
                                    RecordType = RecordType.Credit

                                };

                                _dbContext.Transactions.Add(transaction);
                            }

                            await _dbContext.SaveChangesAsync();
                            if (emailTemplate != null)
                            {
                                var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
                                await _communicationService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", customer.FullName).Replace("[ACCOUNT_NUMBER]", customer.AccountNumber).Replace("[AMOUNT]", amount.ToString()).Replace("[DATE]", trx.CreatedDate.Date.ToString()).Replace("[NARRATION]", trx.Narration).Replace("[ACCOUNT_BALANCE]", wallet.Balance.ToString()).Replace("[TRANSACTION_TYPE]", "Credit").Replace("[TRANSACTION_REFERENCE]", trx.TransactionReference).Replace("[TIME]", trx.CreatedDate.TimeOfDay.ToString()));
                            }
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