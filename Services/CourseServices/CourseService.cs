using CC_Karriarpartner.Data;
using CC_Karriarpartner.DTOs;
using CC_Karriarpartner.Models;
using CC_Karriarpartner.Services.ICourseServices;
using CC_Karriarpartner.Services.ValidationServices;
using Microsoft.EntityFrameworkCore;

namespace CC_Karriarpartner.Services.CourseServices
{
    public class CourseService : ICourseService
    {
        private readonly KarriarPartnerDBContext context;
        private readonly IValidationService validationService;

        public CourseService(KarriarPartnerDBContext context, IValidationService validationService)
        {
            this.context = context;
            this.validationService = validationService;
        }

        public async Task<List<CourseDto>> GetAllCourses()
        {
            var courses = await context.Courses
                .Include(c => c.Videos)
                .Include(c => c.Reviews)
                .Include(c => c.Certificates)
                .Where(c => c.Active)
                .ToListAsync();

            var courseDtos = courses.Select(course => new CourseDto
            {
                Id = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                Category = course.Category,
                Price = course.Price,
                Active = course.Active,
                IsCompleted = course.Completed,

                Videos = course.Videos?.Select(v => new CourseVideoDto
                {
                    Title = v.Title,
                    VideoUrl = v.VideoUrl,
                    IsActive = v.IsActive
                }).ToList()
            }).ToList(); 

            return courseDtos;
        }



        public async Task<CourseDto> GetCourseById(int id)
        {
            var course = await context.Courses.FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
            {
                throw new KeyNotFoundException("Kursen existerar ej.");
            }

            var courseDto = new CourseDto
            {
                Id = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                Category = course.Category,
                Price = course.Price,
                Active = course.Active,
                IsCompleted = course.Completed
            };

            return courseDto;
        }

        public async Task<(bool Success, CourseDto? CourseDto, List<string> ErrorMessages)> CreateCourse(CourseDto courseDto)
        {
            // Validate the courseDto
            var (isValid, errorMessages) = validationService.ValidateModel(courseDto);
            if (!isValid)
            {
                return (false, null, errorMessages);
            }

            var course = new Course
            {
                Title = courseDto.Title,
                Description = courseDto.Description,
                Category = courseDto.Category,
                Price = courseDto.Price,
                Active = courseDto.Active,
                Completed = courseDto.IsCompleted,
                Level = "Beginner" // Default value - adjust as needed
            };

            context.Courses.Add(course);
            await context.SaveChangesAsync();

            courseDto.Id = course.CourseId;
            return (true, courseDto, new List<string>());
        }

        public async Task<(bool Success, List<string> ErrorMessages)> UpdateCourse(int id, CourseDto courseDto)
        {
            // Validate the courseDto
            var (isValid, errorMessages) = validationService.ValidateModel(courseDto);
            if (!isValid)
            {
                return (false, errorMessages);
            }

            var course = await context.Courses.FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
            {
                return (false, new List<string> { "Kursen existerar ej." });
            }

            course.Title = courseDto.Title;
            course.Description = courseDto.Description;
            course.Category = courseDto.Category;
            course.Price = courseDto.Price;
            course.Active = courseDto.Active;
            course.Completed = courseDto.IsCompleted;
            course.Videos = courseDto.Videos?.Select(v => new CourseVideo
            {
                Title = v.Title,
                VideoUrl = v.VideoUrl,
                IsActive = v.IsActive
            }).ToList();

            try
            {
                await context.SaveChangesAsync();
                return (true, new List<string>());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return (false, new List<string> { "Kursen existerar ej." });
                }
                throw;
            }
        }

        public async Task<(bool Success, List<string> ErrorMessages)> DeleteCourse(int id)
        {
            var course = await context.Courses.FindAsync(id);

            if (course == null)
            {
                return (false, new List<string> { "Kursen existerar ej." });
            }

            context.Courses.Remove(course);
            await context.SaveChangesAsync();

            return (true, new List<string>());
        }

        public async Task<bool> AddVideoAsync(CreateCourseVideoDto dto)
        {
            var video = new CourseVideo
            {
                CourseId_FK = dto.CourseId,
                Title = dto.Title,
                VideoUrl = dto.VideoUrl,
                IsActive = dto.IsActive
            };

            context.CourseVideos.Add(video);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddReviewAsync(CreateCourseReviewDto dto)
        {
            var review = new CourseReview
            {
                CourseId_FK = dto.CourseId,
                UserId_FK = dto.UserId,
                Comments = dto.Comments,
                Rating = dto.Rating
            };

            context.CourseReviews.Add(review);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddCertificateAsync(CreateCertificateDto dto)
        {
            var cert = new Certificate
            {
                CourseId_FK = dto.CourseId,
                UserId_FK = dto.UserId,
                CertificateUrl = dto.CertificateUrl,
                IssuedAt = dto.IssuedAt
            };

            context.Certificates.Add(cert);
            await context.SaveChangesAsync();
            return true;
        }


        private bool CourseExists(int id)
        {
            return context.Courses.Any(c => c.CourseId == id);
        }
    }
}
