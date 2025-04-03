namespace CC_Karriarpartner.DTOs.CourseDtos
{
    public class CreateCourseReviewDto
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }
    }
}
