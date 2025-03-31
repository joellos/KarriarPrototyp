using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.Models
{
    public class Certificate
    {
        [Key]
        public int CertificateId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId_FK { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId_FK { get; set; }

        [Required]
        [MaxLength(500)]
        [Url]
        public string CertificateUrl { get; set; }

        [Required]
        public DateTime IssuedAt { get; set; }

        public virtual User User { get; set; }
        public virtual Course Course { get; set; }
    }
}
