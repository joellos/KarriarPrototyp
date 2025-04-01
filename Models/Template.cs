using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.Models
{
    public class Template
    {
        [Key]
        public int TemplateId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; }

        [Required]
        [MaxLength(50)]
        public string Level { get; set; }

        [Required]
        [Range(0, 9999.99)]
        public decimal Price { get; set; }

        public bool Active { get; set; } = true;

        [Required]
        [Url]
        public string PdfUrl { get; set; }

        public virtual List<PurchaseItem> PurchaseItems { get; set; } = new();
    }
}
