using System;
using System.Security.Cryptography;
using System.Text;
using Awacash.Domain.Common.Constants;
using Awacash.Domain.Entities;
using Awacash.Domain.Enums;
using Awacash.Domain.Helpers;
using Awacash.Domain.Interfaces;
using Awacash.Shared;
using Microsoft.EntityFrameworkCore;
using TransactionQueryJob.Data;
using TransactionQueryJob.Interfaces;

namespace TransactionQueryJob
{
    public class SavingsWorker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly AppDbContext _dbContext;
        private readonly ICommunicationService _communicationService;
        private readonly IBankOneAccountService _bankOneAccountService;
        public SavingsWorker(ILogger<Worker> logger, AppDbContext dbContext, ICommunicationService communicationService, IBankOneAccountService bankOneAccountService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _communicationService = communicationService;
            _bankOneAccountService = bankOneAccountService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessRequest()
        {
            try
            {
                _logger.LogInformation("Calling Account Transaction data at: {time}", DateTimeOffset.Now);
                var savings = await _dbContext.Savings.Where(x => x.MaturityDate <= DateTime.Today && x.SavingType != SavingType.AWAFIXED && x.NextPaymentDate == DateTime.Today).ToListAsync();
                if (savings != null)
                {
                    var emailTemplate = await _dbContext.EmailTemplate.Where(x => x.EmailType == EmailType.Transaction).FirstOrDefaultAsync();


                    foreach (var saving in savings)
                    {
                        _logger.LogInformation("Calling Customer data at: {time}", DateTimeOffset.Now);

                        var customer = await _dbContext.Customers.SingleOrDefaultAsync(x => x.Id == saving.CustomerId);
                        if (customer != null)
                        {

                            //validate debit account

                            var debitAccountNameEnquiry = await _bankOneAccountService.GetAccountsByAccountNumber(saving.DeductionAccount);
                            if (debitAccountNameEnquiry == null || debitAccountNameEnquiry.Data == null || !debitAccountNameEnquiry.IsSuccessful)
                            {
                                continue;
                            }

                            // validate credit account
                            var creditAccountNameEnquiry = await _bankOneAccountService.GetAccountsByAccountNumber(saving.SavingAccount);
                            if (creditAccountNameEnquiry == null || creditAccountNameEnquiry.Data == null || !debitAccountNameEnquiry.IsSuccessful)
                            {
                                continue;
                            }

                            // check if account belongs to same customer

                            if (!debitAccountNameEnquiry.Data.CustomerID.Equals(creditAccountNameEnquiry.Data.CustomerID))
                            {
                                continue;
                            }
                            // check sufficient balance
                            var debitaccountBalance = await _bankOneAccountService.BalanceEnquiry(saving.DeductionAccount);
                            if (debitaccountBalance == null || debitaccountBalance.Data == null || !debitaccountBalance.IsSuccessful)
                            {
                                continue;

                            }
                            // transfer to credit account
                            decimal.TryParse(saving.DeductionAccount, out decimal amount);
                            var tranRef = GenerateNumericKey(12);
                            var response = await _bankOneAccountService.IntraBankTransfer(amount, saving.DeductionAccount, saving.SavingAccount, "", "mobile", tranRef);
                            if (response == null || response.Data == null || !response.IsSuccessful)
                            {
                                continue;

                            }
                            if (!response.Data.IsSuccessful)
                            {
                                continue;
                            }


                            // update savings next payment date
                            saving.NextPaymentDate = CalNextPayment(saving.DeductionFrequency);
                            saving.ModifiedDate = DateTime.UtcNow;
                            _dbContext.Update(saving);
                            await _dbContext.SaveChangesAsync();
                            // save transaction
                            var savingTransaction = new Transaction()
                            {
                                CustomerId = customer.Id,
                                Amount = amount,
                                DebitAccountName = debitAccountNameEnquiry.Data.Name,
                                DebitAccountNumber = saving.DeductionAccount,
                                CreditAccountName = creditAccountNameEnquiry.Data.Name,
                                CreditAccountNumber = saving.SavingAccount,
                                Fee = 0,
                                Narration = "",
                                Currency = "NGN",
                                TransactionReference = tranRef,
                                TransactionType = TransactionType.InterBankTransfer,
                                RecordType = RecordType.Debit,
                                Status = response.Data.IsSuccessful ? TransactionStatus.SUCCESSFUL : TransactionStatus.FAILED
                            };
                            // send notification
                            _dbContext.Transactions.Add(savingTransaction);
                            await _dbContext.SaveChangesAsync();
                            if (emailTemplate != null)
                            {
                                var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
                                await _communicationService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", customer.FullName).Replace("[ACCOUNT_NUMBER]", customer.AccountNumber).Replace("[AMOUNT]", amount.ToString()).Replace("[DATE]", savingTransaction.CreatedDate.Date.ToString()).Replace("[NARRATION]", savingTransaction.Narration).Replace("[ACCOUNT_BALANCE]", debitaccountBalance.Data.AvailableBalance.ToString()).Replace("[TRANSACTION_TYPE]", "Credit").Replace("[TRANSACTION_REFERENCE]", tranRef).Replace("[TIME]", savingTransaction.CreatedDate.TimeOfDay.ToString()));
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

        private static string GenerateNumericKey(int size)
        {
            char[] chars =
                "1234567890".ToCharArray();
            //byte[] data = new byte[size];

            var bytes = RandomNumberGenerator.GetBytes(size);
            //using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            //{
            //    crypto.GetBytes(data);
            //}
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in bytes)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        private static DateTime CalNextPayment(DeductionFrequency deductionFrequency)
        {
            DateTime nextPaymentDate = DateTime.UtcNow;

            if (deductionFrequency == DeductionFrequency.Daily)
            {

                nextPaymentDate = DateTime.UtcNow.AddDays(1);
            }
            else if (deductionFrequency == DeductionFrequency.Weekly)
            {
                nextPaymentDate = DateTime.UtcNow.AddDays(7);
            }
            else if (deductionFrequency == DeductionFrequency.Monthly)
            {
                nextPaymentDate = DateTime.UtcNow.AddMonths(1);
            }
            return nextPaymentDate;
        }
    }
}

