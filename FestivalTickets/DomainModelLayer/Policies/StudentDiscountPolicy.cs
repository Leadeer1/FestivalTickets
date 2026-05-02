using System;
using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;
using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Policies
{
    /// <summary>
    /// Polityka rabatowa dla studentów – 50% zniżki.
    ///
    /// Stosowana gdy CustomerType.IsStudent() == true.
    /// DiscountPolicyFactory wybiera tę politykę na podstawie CustomerType.
    /// </summary>
    public class StudentDiscountPolicy : IDiscountPolicy
    {
        private const decimal DiscountPercent = 50m;

        public string Name => $"Zniżka studencka ({DiscountPercent}%)";

        public Money CalculateDiscount(Money basePrice, DateTime purchasedAt, EventDay eventDay)
        {
            if (basePrice == null) throw new ArgumentNullException(nameof(basePrice));
            return basePrice.DiscountAmount(DiscountPercent);
        }
    }
}
