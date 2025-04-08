using CC_Karriarpartner.Data;
using Microsoft.EntityFrameworkCore;

namespace CC_Karriarpartner.Endpoints.SearchEndpoints
{
    public class TemplateSearchEndpoint
    {
        public static void RegisterTemplateSearchEndpoint(WebApplication app)
        {
            app.MapGet("/api/SearchForTemplates", async (string? search, string? category, string? price, string? level, KarriarPartnerDBContext context,
                int page = 1, int pageSize = 10) => 
            {
                var templateList = context.Templates.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    templateList = templateList.Where(t =>
                        t.Title.Contains(search) ||
                        t.Description.Contains(search));
                }

                if (!string.IsNullOrWhiteSpace(category))
                {
                    templateList = templateList.Where(t => t.Category == category);
                }

                if (!string.IsNullOrWhiteSpace(level))
                {
                    templateList = templateList.Where(t => t.Level == level);
                }

                if (!string.IsNullOrWhiteSpace(price))
                {
                    switch (price)
                    {
                        case "Gratis":
                            templateList = templateList.Where(t => t.Price == 0);
                            break;

                        case "Under 100 kr":
                            templateList = templateList.Where(t => t.Price < 100);
                            break;

                        case "100 - 300 kr":
                           templateList = templateList.Where(t => t.Price >= 100 && t.Price <= 300);
                            break;

                        case "Över 300 kr":
                           templateList = templateList.Where(t => t.Price > 300);
                            break;
                    }
                }

                var templates = await templateList
                    .Skip((page - 1) * pageSize) 
                    .Take(pageSize)
                    .ToListAsync();

                return Results.Ok(templates);

            }).WithTags("Templates");
        }
    }
}
