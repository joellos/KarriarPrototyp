using CC_Karriarpartner.Services.IAdminServices;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CC_Karriarpartner.Endpoints.AdminPanelEndpoints
{
    public class GetPurchasesEndpoint
    {
        public static void PurchasesEndpoints(WebApplication app)
        {
            app.MapGet("/api/purchases", async (IAdminPanel service) =>
                {
                    try
                    {
                        var purchases = await service.GetAllPurchases();
                        return Results.Ok(purchases);
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem(
                            title: "Error",
                            detail: ex.Message,
                            statusCode: 500);
                    }
                });
        }
    }
}
