using System;
using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;
using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Interfaces
{
    /// <summary>
    /// Interfejs polityki rabatowej – wzorzec Strategy.
    ///
    /// Każda implementacja to osobny plik w katalogu Policies/.
    /// Nowy rodzaj rabatu = nowy plik + jedna linijka w rejestracji DI.
    /// Zero zmian w reszcie kodu (Open/Closed Principle).
    ///
    /// Implementacje:
    ///   StudentDiscountPolicy        – 50% dla studentów
    ///   KrakowResidentDiscountPolicy – 20% dla mieszkańców Krakowa
    ///   EarlyBirdDiscountPolicy      – 15% przy zakupie >30 dni przed wydarzeniem
    ///   RegularPricePolicy           – 0%, fallback (Null Object Pattern)
    ///
    /// Dziedziczy po IFestivalPolicy – wszystkie polityki festiwalowe mają wspólny marker.
    /// </summary>
    public interface IDiscountPolicy : IFestivalPolicy
    {
        string Name { get; }

        /// <summary>
        /// Oblicza kwotę rabatu (nie cenę po rabacie).
        /// </summary>
        /// <param name="basePrice">Cena bazowa biletu (przed rabatem)</param>
        /// <param name="purchasedAt">Data zakupu – używana przez EarlyBirdDiscountPolicy</param>
        /// <param name="eventDay">Dzień festiwalu – używana przez EarlyBirdDiscountPolicy</param>
        /// <returns>Kwota rabatu. Nigdy więcej niż basePrice.</returns>
        Money CalculateDiscount(Money basePrice, DateTime purchasedAt, EventDay eventDay);
    }
}
