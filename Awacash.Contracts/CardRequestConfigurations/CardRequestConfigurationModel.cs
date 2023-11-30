using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awacash.Domain.Enums;

namespace Awacash.Contracts.CardRequestConfigurations
{
    public class CardRequestConfigurationModel
    {
        public string IssuerName { get; set; }
        public decimal Price { get; set; }
        public CardType CardType { get; set; }
    }
}
