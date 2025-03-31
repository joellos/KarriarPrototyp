using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.Models
{
    public class UserSubscriptions
    {
        [Key]
        public int SubscriptionId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId_FK { get; set; }

        [Required]
        public DateTime StartDate { get; set; } // Dag och Tid

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [MaxLength(10)]
        public string Status { get; set; }

        [Required]
        [MaxLength(30)]
        public string PaymentType { get; set; } = "Klarna";

        public virtual User User { get; set; }
    }
}
