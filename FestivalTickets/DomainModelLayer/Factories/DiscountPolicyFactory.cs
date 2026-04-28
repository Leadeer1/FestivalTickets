using System;
using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using DDD.FestivalTickets.Core.DomainModelLayer.Policies;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Factories
{
    /// <summary>
    /// Fabryka polityk rabatowych.
    /// Odpowiednik DiscountPolicyFactory z EscapeRoom.
    ///
    /// Na podstawie CustomerType klienta wybiera właściwą politykę rabatową.
    /// Logika wyboru jest TUTAJ – nie w Ticket, nie w CommandHandlerze.
    ///
    /// Kolejność priorytetów (ważne – student > krakowianin > early bird > regularna):
    ///   1. Student        – 50% (najwyższy priorytet, dominuje nad innymi)
    ///   2. KrakowResident – 20%
    ///   3. EarlyBird      – 15% (sprawdzany dla Regular jeśli kupuje wcześnie)
    ///   4. Regular        – 0%, fallback
    ///
    /// Jeśli chcesz kumulować rabaty (student + early bird) zmień metodę Create()
    /// na zwracanie listy polityk i zsumuj w Ticket.Purchase() – bez zmian w interfejsie.
    /// </summary>
    public class DiscountPolicyFactory
    {
        private readonly EarlyBirdDiscountPolicy      _earlyBird;
        private readonly StudentDiscountPolicy        _student;
        private readonly KrakowResidentDiscountPolicy _krakowResident;
        private readonly RegularPricePolicy           _regular;

        public DiscountPolicyFactory()
        {
            _earlyBird      = new EarlyBirdDiscountPolicy();
            _student        = new StudentDiscountPolicy();
            _krakowResident = new KrakowResidentDiscountPolicy();
            _regular        = new RegularPricePolicy();
        }

        /// <summary>
        /// Wybiera politykę rabatową na podstawie typu klienta.
        /// Nigdy nie zwraca null – w najgorszym razie RegularPricePolicy.
        /// </summary>
        public IDiscountPolicy Create(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            if (customer.CustomerType.IsStudent())
                return _student;

            if (customer.CustomerType.IsKrakowResident())
                return _krakowResident;

            // zwykły uczestnik – sprawdź czy kwalifikuje się do early bird
            // (early bird jest bonusem tylko dla Regular – student i krakowianin
            //  mają już lepsze rabaty i tak)
            return _earlyBird;
        }
    }
}
