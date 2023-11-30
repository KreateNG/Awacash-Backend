using System;
using Awacash.Domain.Entities;
using Awacash.Domain.Enums;
using Awacash.Domain.Specifications;

namespace Awacash.Application.CardRequests.Specifications
{
    public class CardRequestFilterSpecification : BaseSpecification<CardRequest>
    {
        public CardRequestFilterSpecification(string? cardName, CardType? cardType, CardDeliveryStatus? deliveryStatus)
            : base(
                f =>
                  (string.IsNullOrWhiteSpace(cardName) || f.CardName.ToLower() == cardName.ToLower()) &&
                  (!cardType.HasValue || f.CardType == cardType) &&
                  (!deliveryStatus.HasValue || f.DeliveryStatus == deliveryStatus)
            )
        {

        }
    }
}

