using System;
using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;
using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Policies
{
    /// <summary>
    /// Polityka early bird – 15% zniżki przy zakupie biletu
    /// co najmniej 30 dni przed wydarzeniem.
    ///
    /// Jedyna polityka która zależy od daty zakupu i daty wydarzenia.
    /// Dlatego IDiscountPolicy.CalculateDiscount() przyjmuje purchasedAt i eventDay.
    ///
    /// Może być łączona z innymi politykami – DiscountPolicyFactory decyduje o kolejności.
    /// </summary>
    public class EarlyBirdDiscountPolicy : IDiscountPolicy
    {
        private const decimal DiscountPercent    = 15m;
        private const int     DaysBeforeRequired = 30;

        public string Name => $"Early Bird – zakup {DaysBeforeRequired}+ dni wcześniej ({DiscountPercent}%)";

        public Money CalculateDiscount(Money basePrice, DateTime purchasedAt, EventDay eventDay)
        {
            if (basePrice  == null) throw new ArgumentNullException(nameof(basePrice));
            if (eventDay   == null) throw new ArgumentNullException(nameof(eventDay));

            int daysUntilEvent = (eventDay.Date - purchasedAt.Date).Days;

            if (daysUntilEvent >= DaysBeforeRequired)
                return basePrice.DiscountAmount(DiscountPercent);

            return Money.Zero;
        }
    }
}
