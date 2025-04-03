using CC_Karriarpartner.DTOs;

namespace CC_Karriarpartner.Services.ICourseServices
{
    public interface ICourseService
    {
        Task<List<CourseDto>> GetAllCourses();
        Task<CourseDto> GetCourseById(int id);
        Task<(bool Success, CourseDto? CourseDto, List<string> ErrorMessages)> CreateCourse(CourseDto courseDto);
        Task<(bool Success, List<string> ErrorMessages)> UpdateCourse(int id, CourseDto courseDto);
        Task<(bool Success, List<string> ErrorMessages)> DeleteCourse(int id);
    }
}