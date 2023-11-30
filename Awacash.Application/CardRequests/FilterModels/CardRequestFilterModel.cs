using System;
using Awacash.Domain.Entities;
using Awacash.Domain.Enums;
using Awacash.Domain.FilterModels;

namespace Awacash.Application.CardRequests.FilterModels
{
    public class CardRequestFilterModel : BaseFilterModel
    {
        public string? CardName { get; set; }
        public CardType? CardType { get; set; }
        public CardDeliveryStatus? DeliveryStatus { get; set; }
    }
}

