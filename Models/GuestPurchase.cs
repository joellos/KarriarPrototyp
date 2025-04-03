using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.Models
{
    public enum PaymentStatus
    {
        Väntar,
        Betald,
        Misslyckad
    }

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
        public PaymentStatus PaymentStatus { get; set; }

        public Template? Template { get; set; }

    }
}
