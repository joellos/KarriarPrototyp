using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.Models
{
    public class GuestPurchase
    {
        [Key]
        public int GuestId { get; set; }

        [Required]
        [EmailAddress]
        public string GuestEmail { get; set; }

        [Required]
        [ForeignKey("Template")]
        public int TemplateId_Fk { get; set; }

        [Required]
        public DateTime PurchaseTime { get; set; }

        [Required]
        public string PaymentStatus { get; set; }

        public Template? Template { get; set; }

    }
}
