using System;
using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;
using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Policies
{
    /// <summary>
    /// Polityka rabatowa dla mieszkańców Krakowa – 20% zniżki.
    ///
    /// Stosowana gdy CustomerType.IsKrakowResident() == true.
    /// </summary>
    public class KrakowResidentDiscountPolicy : IDiscountPolicy
    {
        private const decimal DiscountPercent = 20m;

        public string Name => $"Zniżka dla mieszkańca Krakowa ({DiscountPercent}%)";

        public Money CalculateDiscount(Money basePrice, DateTime purchasedAt, EventDay eventDay)
        {
            if (basePrice == null) throw new ArgumentNullException(nameof(basePrice));
            return basePrice.DiscountAmount(DiscountPercent);
        }
    }
}
