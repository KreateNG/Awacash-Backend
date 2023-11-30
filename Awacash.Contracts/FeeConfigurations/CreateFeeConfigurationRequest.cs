using System;
using Awacash.Domain.Enums;

namespace Awacash.Contracts.FeeConfigurations
{
    public record CreateFeeConfigurationRequest(TransactionType? TransactionType, decimal? UpperBound, decimal? LowerBound, decimal? Fee);
}

