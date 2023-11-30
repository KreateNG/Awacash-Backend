using System;
namespace Awacash.Infrastructure.Jobs;



public static class JobStartup
{
    public static void AddJobs()
    {
        //RecurringJob.AddOrUpdate<IVerificationJobService>("VerifyUserBVN",
        //    x => x.VerifyUserBVN(), "*/30 * * * *");

        //RecurringJob.AddOrUpdate<IVerificationJobService>("VerifyUsersNIN",
        //    x => x.VerifyUsersNIN(), "*/33 * * * *");

        //RecurringJob.AddOrUpdate<IProvidusTransactionService>("ProcessFundAccountRequest",
        //    x => x.ProcessFundAccountRequest(), "*/5 * * * *");

        //RecurringJob.AddOrUpdate<ILoanJobService>("ProcessRepayment",
        //    x => x.ProcessRepayment(), "0 23 * * *");

        //RecurringJob.AddOrUpdate<ILoanJobService>("DisburseApprovedLoans",
        //    x => x.DisburseApprovedLoans(), "*/3 * * * *");
    }
}


