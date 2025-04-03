namespace CC_Karriarpartner.DTOs
{
    public class CreateCourseReviewDto
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }
    }
}
