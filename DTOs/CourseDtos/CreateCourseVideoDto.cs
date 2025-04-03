namespace CC_Karriarpartner.DTOs.CourseDtos
{
    public class CreateCourseVideoDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

