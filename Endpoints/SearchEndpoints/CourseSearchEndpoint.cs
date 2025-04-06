using CC_Karriarpartner.Data;
using Microsoft.EntityFrameworkCore;

namespace CC_Karriarpartner.Endpoints.SearchEndpoints
{
    public class CourseSearchEndpoint
    {
        public static void RegisterCourseSearchEnpoints(WebApplication app)
        {
            app.MapGet("/api/SearchForCourses", async (string? search, string? category, string? price, string? level, KarriarPartnerDBContext context,
                int page = 1, int pageSize = 10) => // Page size för frontend, hur många kurser som laddas upp samtidigt!
            {
                var courseList = context.Courses.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    courseList = courseList.Where(c =>
                        c.Title.Contains(search) ||
                        c.Description.Contains(search));
                }

                if (!string.IsNullOrWhiteSpace(category))
                {
                    courseList = courseList.Where(c => c.Category == category);
                }

                if (!string.IsNullOrWhiteSpace(level))
                {
                    courseList = courseList.Where(c => c.Level == level);
                }

                if (!string.IsNullOrWhiteSpace(price))
                {
                    switch (price)
                    {
                        case "Gratis":
                            courseList = courseList.Where(c => c.Price == 0);
                            break;

                        case "Under 100 kr":
                            courseList = courseList.Where(c => c.Price < 100);
                            break;

                        case "100 - 300 kr":
                            courseList = courseList.Where(c => c.Price >= 100 && c.Price <= 300);
                            break;

                        case "Över 300 kr":
                            courseList = courseList.Where(c => c.Price > 300);
                            break;
                    }
                }

                var courses = await courseList
                    .Skip((page - 1) * pageSize) // Gör att knappen "nästa sida" i frontend blir funktionell!
                    .Take(pageSize)
                    .ToListAsync();

                return Results.Ok(courses);

            });
        }
    }
}
