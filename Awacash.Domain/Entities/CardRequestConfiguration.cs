using Awacash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Entities
{
    public class CardRequestConfiguration: BaseEntity
    {
        public string? IssuerName { get; set; }
        public decimal Price { get; set; }
        public CardType CardType { get; set; }
        public Status Status { get; set; }
    }
}
