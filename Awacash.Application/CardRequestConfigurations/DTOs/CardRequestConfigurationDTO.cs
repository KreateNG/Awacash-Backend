using Awacash.Application.Common.Model;
using Awacash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.CardRequestConfigurations.DTOs
{
    public class CardRequestConfigurationDTO: BaseDTO
    {
        public string? IssuerName { get; set; }
        public decimal Price { get; set; }
        public CardType CardType { get; set; }
        public Status Status { get; set; }
    }
}
