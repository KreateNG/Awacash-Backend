using Awacash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Entities
{
    public class CardRequest: BaseEntity
    {
        public string? CardName { get; set; }
        public CardType CardType { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public CardDeliveryStatus DeliveryStatus { get; set; }


    }
}
