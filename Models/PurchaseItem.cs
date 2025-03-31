using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.Models
{
    public class PurchaseItem
    {
        [Key]
        public int PurchaseItemId { get; set; }

        [ForeignKey("Course")]
        public int? CourseId_Fk { get; set; } // Bara en av dessa används

        [ForeignKey("Template")]
        public int? TemplateId_Fk { get; set; } // Bara en användsi varje tabell

        [Required]
        [ForeignKey("Purchase")]
        public int PurchaseId_Fk { get; set; }
        
        public virtual Purchase Purchase { get; set; }
        public virtual Course? Course { get; set; }
        public virtual Template? Template { get; set; }
    }
}
