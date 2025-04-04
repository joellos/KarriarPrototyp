using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.DTOs.AdminPanelDtos
{
    public class PurchaseDto
    {
        [Required]
        [Range(0, 99999.99)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public DateTime BuyDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string TransactionId { get; set; }
        public List<PurchaseItemDto> Items { get; set; }

    }
}
