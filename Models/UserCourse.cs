using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.Models
{
    public class UserCourse
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public bool Completed { get; set; } = false;
        public DateTime? CompletedDate { get; set; }

        public virtual User User { get; set; }
        public virtual Course Course { get; set; }
    }
}
