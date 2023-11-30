using System;
using Awacash.Application.Common.Model;
using Awacash.Domain.Enums;

namespace Awacash.Application.FeeConfigurations.DTOs
{
    public class FeeConfigurationDto : BaseDTO
    {
        public TransactionType TransactionType { get; set; }

        public decimal UpperBound { get; set; }

        public decimal LowerBound { get; set; }

        public decimal Fee { get; set; }
    }
}

