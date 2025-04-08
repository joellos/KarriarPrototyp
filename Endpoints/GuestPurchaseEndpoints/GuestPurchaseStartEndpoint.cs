using CC_Karriarpartner.Data;
using CC_Karriarpartner.DTOs.GuestPurchaseDTO;
using CC_Karriarpartner.Models;

namespace CC_Karriarpartner.Endpoints.GuestPurchaseEndpoints
{
    public class GuestPurchaseStartEndpoint
    {
        public static void RegisterGuestPurchaseStartEndpoint(WebApplication app)
        {
            app.MapPost("/api/guestpurchase/start", async (KarriarPartnerDBContext context, GuestPurchaseRequestDTO request) =>
            {
                var product = await context.Templates.FindAsync(request.TemplateId_Fk);
                if (product == null)
                {
                    return Results.NotFound("Produkten hittades inte!");
                }

                var newPurchase = new GuestPurchase
                {
                    GuestEmail = request.GuestEmail,
                    TemplateId_Fk = request.TemplateId_Fk,
                    PurchaseTime = DateTime.Now,
                    PaymentStatus = PaymentStatus.Väntar
                };

                context.GuestPurchases.Add(newPurchase);
                await context.SaveChangesAsync();

                return Results.Ok(new
                {
                    GuestPurchaseId = newPurchase.GuestId
                });

            });
        }
    }
}
