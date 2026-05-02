using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Interfaces
{
    /// <summary>
    /// Serwis infrastrukturalny do wysyłania powiadomień.
    /// Odpowiednik IEmailDispatcher z projektu EscapeRoom.
    ///
    /// Interfejs zdefiniowany w warstwie domenowej – implementacja w InfrastructureLayer.
    /// Dependency Inversion: domena zna abstrakcję, nie konkret.
    ///
    /// Implementacja konsolowa (NotificationService.cs) po prostu wypisuje na konsolę.
    /// W produkcji można podmienić na SMTP, SendGrid itp. bez zmiany domeny.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Wysyła potwierdzenie zakupu biletu.
        /// Wywoływane przez handler zdarzenia TicketPurchasedDomainEvent.
        /// </summary>
        void SendPurchaseConfirmation(long customerId, long ticketId, Money finalPrice);

        /// <summary>
        /// Wysyła powiadomienie o anulowaniu biletu.
        /// </summary>
        void SendCancellationNotification(long customerId, long ticketId);
    }
}
