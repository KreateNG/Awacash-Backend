using Awacash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Contracts.CardRequests
{
    public record CardRequestModel(string? AccountNumber, string? CardName, CardType? CardType, string? DeliveryAddress, string? CardConfigId);
}
