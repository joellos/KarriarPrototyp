using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Karriarpartner.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(35)]
        public string Category { get; set; }

        [Required]
        [MaxLength(35)]
        public string Level { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 9999.99)] // Om den ingår i premenation?
        public decimal Price { get; set; }

        public bool Active { get; set; } = true; // True som default??

        public virtual List<CourseVideo> Videos { get; set; }
        public virtual List<CourseReview> Reviews { get; set; }
        public virtual List<Certificate> Certificates { get; set; }
        public virtual List<PurchaseItem> PurchaseItems { get; set; }
    }
}
