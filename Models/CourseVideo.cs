using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.Models
{
    public class CourseVideo
    {
        [Key]
        public int CourseVideoId { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId_FK { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(300)] // För mycket??
        [Url]
        public string VideoUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual Course Course { get; set; }
    }
}
