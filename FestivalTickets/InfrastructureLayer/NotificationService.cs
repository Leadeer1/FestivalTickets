using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.InfrastructureLayer
{
    /// <summary>
    /// Implementacja konsolowa INotificationService.
    /// W produkcji podmień na SMTP / SendGrid – zero zmian w domenie i Application Layer.
    /// </summary>
    public class NotificationService : INotificationService
    {
        public void SendPurchaseConfirmation(long customerId, long ticketId, Money finalPrice)
        {
            Console.WriteLine(
                $"[NOTIFICATION] Potwierdzenie zakupu – " +
                $"Klient: {customerId} | Bilet: {ticketId} | Cena: {finalPrice.Amount} {finalPrice.Currency}");
        }

        public void SendCancellationNotification(long customerId, long ticketId)
        {
            Console.WriteLine(
                $"[NOTIFICATION] Anulowanie biletu – " +
                $"Klient: {customerId} | Bilet: {ticketId}");
        }
    }
}
