using Awacash.Application.Common.Model;
using Awacash.Domain.Entities;
using Awacash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.CardRequests.DTOs
{
    public class CardRequestDTO : BaseDTO
    {
        public string? CardName { get; set; }
        public CardType CardType { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? CustomerId { get; set; }
        public CustomerDTO? Customer { get; set; }
        public CardDeliveryStatus DeliveryStatus { get; set; }
    }
}
