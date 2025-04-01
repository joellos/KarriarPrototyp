using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId_FK { get; set; }

        [Required]
        [Range(0, 99999.99)]
        public decimal Price { get; set; }

        [Required]
        public DateTime BuyDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string TransactionId { get; set; }

        public virtual User User { get; set; }
        public virtual List<PurchaseItem> PurchaseItems { get; set; }
    }
}
