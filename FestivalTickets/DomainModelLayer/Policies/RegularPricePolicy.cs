using System;
using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;
using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Policies
{
    /// <summary>
    /// Polityka braku rabatu – Null Object Pattern.
    /// Zwraca zawsze Money.Zero jako rabat.
    ///
    /// Używana jako fallback gdy żadna inna polityka nie pasuje.
    /// Eliminuje potrzebę sprawdzania null w Ticket.Purchase().
    /// </summary>
    public class RegularPricePolicy : IDiscountPolicy
    {
        public string Name => "Cena regularna (brak rabatu)";

        public Money CalculateDiscount(Money basePrice, DateTime purchasedAt, EventDay eventDay)
        {
            return Money.Zero;
        }
    }
}
