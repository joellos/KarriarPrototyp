using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(10)]
        public string Category { get; set; }

        [Required]
        [Range(0, 9999.99)]
        public decimal Price { get; set; }

        public bool Active { get; set; } = true;
        public bool IsCompleted { get; set; } = false;
        public List<CourseVideoDto>? Videos { get; set; }
        public List<CourseReviewDto>? Reviews { get; set; }
        public List<CertificateDto>? Certificates { get; set; }
'

    }
}
