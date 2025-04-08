using CC_Karriarpartner.Services.IAdminServices;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace CC_Karriarpartner.Endpoints.AdminPanelEndpoints
{
    public class GetPurchasesEndpoint
    {
        public static void PurchasesEndpoints(WebApplication app)
        {
            app.MapGet("/api/purchases", async (ClaimsPrincipal user,IAdminPanel service, int page = 1, int pageSize = 10) =>
                {
                    if(!user.IsInRole("Admin"))
                    {
                        return Results.Unauthorized();

                    }

                    try
                    {
                        var (purchases, totalCount) = await service.GetAllPurchases(page, pageSize);

                        return Results.Ok(new
                        {
                            items = purchases,
                            totalCount = totalCount,
                            page = page,
                            pageSize = pageSize,
                            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                        });
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem(
                            title: "Error",
                            detail: ex.Message,
                            statusCode: 500);
                    }
                })
                .RequireAuthorization("AdminPolicy")
                .WithName("GetALlPurchasesAdmin")
                .WithDescription("Endpoint to get all purchases made on website, only works with admin auth");
        }
    }
}
