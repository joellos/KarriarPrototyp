using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.Models
{
    public class CourseReview
    {
        [Key]
        public int CourseReviewId { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(1000)]
        public string Comments { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId_FK { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId_FK { get; set; }

        [Required]
        [Range(1, 5)] // Frontend får fixa sjärnorna
        public int Rating { get; set; }

        public virtual Course Course { get; set; }
        public virtual User User { get; set; }
    }
}
