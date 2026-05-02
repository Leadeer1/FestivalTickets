namespace DDD.FestivalTickets.Core.DomainModelLayer.Interfaces
{
    /// <summary>
    /// Interfejs znacznikowy (marker interface) dla wszystkich polityk festiwalowych.
    ///
    /// Pozwala rejestrować w DI wszystkie polityki jako IFestivalPolicy
    /// i filtrować je według konkretnego podinterfejsu (np. IDiscountPolicy).
    ///
    /// Przykład rozszerzenia w przyszłości:
    ///   ICapacityPolicy   : IFestivalPolicy  – polityka limitowania liczby biletów
    ///   IRefundPolicy     : IFestivalPolicy  – polityka zwrotów
    ///
    /// Dzięki temu DiscountPolicyFactory otrzymuje przez DI tylko IDiscountPolicy,
    /// a nie wszystkie polityki festiwalowe naraz.
    /// </summary>
    public interface IFestivalPolicy
    {
        // marker - brak metod celowo
    }
}
