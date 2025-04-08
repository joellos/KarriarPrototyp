using CC_Karriarpartner.Data;
using CC_Karriarpartner.DTOs.GuestPurchaseDTO;
using CC_Karriarpartner.Models;

namespace CC_Karriarpartner.Endpoints.GuestPurchaseEndpoints
{
    public class GuestPurchasePaymentEndpoint
    {
        public static void RegisterGuestPurchasePaymentEndpoint(WebApplication app)
        {
            app.MapPost("/api/guestpurchase/payment", async (KarriarPartnerDBContext context, int guestPurchaseId) =>
            {
                var purchase = await context.GuestPurchases.FindAsync(guestPurchaseId);

                if (purchase == null)
                {
                    purchase.PaymentStatus = PaymentStatus.Misslyckad;
                    return Results.NotFound("Köpet gick inte genom! Testa igen!");
                    
                }

                purchase.PaymentStatus = PaymentStatus.Betald;
                await context.SaveChangesAsync();

                return Results.Ok($"Betalningen gick genom! Mallen skickas nu till: {purchase.GuestEmail}");

            }).WithTags("Payments");
        }
    }
}
