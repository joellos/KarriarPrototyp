using CC_Karriarpartner.Data;
using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CC_Karriarpartner.Endpoints.GenerateDiplomaEndpoints
{
    public class CourseDiplomaEndpoint
    {
        public static void RegisterCourseDiplomaEndpoint(WebApplication app)
        {
            app.MapGet("/api/diploma/{userId:int}/{courceId:int}", async (
                int userId, int courseId, IConverter converter, KarriarPartnerDBContext context) =>
            {
                var user = await context.Users.FindAsync(userId);
                var course = await context.Courses.FindAsync(courseId);

                if (user == null)
                {
                    return Results.BadRequest("Användaren hittades inte!");
                }
                else if (course == null)
                {
                    return Results.BadRequest("Den aktuella kursen hittades inte!");
                }





                var userCourse = await context.UserCourses.FirstOrDefaultAsync(u => u.UserId == userId && u.CourseId == courseId);

                if (userCourse == null)
                {
                    return Results.BadRequest("Ingen kurskoppling kunde hittas!");
                }

                string diplomId = Guid.NewGuid().ToString();

                while (await context.Certificates.AnyAsync(c => c.VerificationId == diplomId))
                {
                    diplomId = Guid.NewGuid().ToString();
                }

                // Behöver hämta html koden!
                // Ersätta placeholders i HTML
                // Generera pdf
                // Spara i databasen
                // returnera pdf nedladdningsbart!

                return Results.Ok();
            });
        }
    }
}
