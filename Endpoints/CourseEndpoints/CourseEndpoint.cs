using CC_Karriarpartner.DTOs;
using CC_Karriarpartner.Services.ICourseServices;

namespace CC_Karriarpartner.Endpoints.CourseEndpoints
{
    public static class CourseEndpoint
    {
        public static void RegisterCourseEndpoints(WebApplication app)
        {
            var courseGroup = app.MapGroup("/api/courses");

            // GET all courses
            courseGroup.MapGet("/", async (ICourseService courseService) =>
            {
                try
                {
                    var courses = await courseService.GetAllCourses();
                    return Results.Ok(courses);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });

            // GET course by id
            courseGroup.MapGet("/{id}", async (int id, ICourseService courseService) =>
            {
                try
                {
                    var course = await courseService.GetCourseById(id);
                    return Results.Ok(course);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new { Message = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });

            // POST create course
            courseGroup.MapPost("/", async (CourseDto courseDto, ICourseService courseService) =>
            {
                var (success, createdCourse, errorMessages) = await courseService.CreateCourse(courseDto);

                if (!success)
                {
                    return Results.BadRequest(new { Errors = errorMessages });
                }

                return Results.Created($"/api/courses/{createdCourse.Id}", createdCourse);
            });

            // PUT update course
            courseGroup.MapPut("/{id}", async (int id, CourseDto courseDto, ICourseService courseService) =>
            {
                var (success, errorMessages) = await courseService.UpdateCourse(id, courseDto);

                if (!success)
                {
                    if (errorMessages.Contains("Kursen existerar ej."))
                    {
                        return Results.NotFound(new { Errors = errorMessages });
                    }

                    return Results.BadRequest(new { Errors = errorMessages });
                }

                return Results.NoContent();
            });


            // DELETE course
            courseGroup.MapDelete("/{id}", async (int id, ICourseService courseService) =>
            {
                var (success, errorMessages) = await courseService.DeleteCourse(id);

                if (!success)
                {
                    return Results.NotFound(new { Errors = errorMessages });
                }

                return Results.NoContent();
            });

            // POST - Add Course Video
            courseGroup.MapPost("/{id}/videos", async (CreateCourseVideoDto dto, ICourseService service) =>
            {
                var result = await service.AddVideoAsync(dto);
                return result ? Results.Ok() : Results.BadRequest();
            });

            // POST - Add Course Review
            courseGroup.MapPost("/{id}/reviews", async (CreateCourseReviewDto dto, ICourseService service) =>
            {
                var result = await service.AddReviewAsync(dto);
                return result ? Results.Ok() : Results.BadRequest();
            });

            // POST - Add Certificate
            courseGroup.MapPost("/{id}/certificates", async (CreateCertificateDto dto, ICourseService service) =>
            {
                var result = await service.AddCertificateAsync(dto);
                return result ? Results.Ok() : Results.BadRequest();
            });

        }
    }
}
