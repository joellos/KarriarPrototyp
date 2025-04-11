using CC_Karriarpartner.DTOs;
using CC_Karriarpartner.DTOs.CourseDtos;

namespace CC_Karriarpartner.Services.ICourseServices
{
    public interface ICourseService
    {
        Task<List<CourseDto>> GetAllCourses();
        Task<CourseDto> GetCourseById(int id);
        Task<(bool Success, CourseDto? CourseDto, List<string> ErrorMessages)> CreateCourse(CourseDto courseDto);
        Task<(bool Success, List<string> ErrorMessages)> UpdateCourse(int id, CourseDto courseDto);
        Task<(bool Success, List<string> ErrorMessages)> DeleteCourse(int id);
        Task<bool> AddVideoAsync(CreateCourseVideoDto dto);
        Task<bool> AddReviewAsync(CreateCourseReviewDto dto);
        Task<bool> AddCertificateAsync(CreateCertificateDto dto);

    }
}