using CC_Karriarpartner.Models;

namespace CC_Karriarpartner.DTOs.GuestPurchaseDTO
{
    public class GuestPurchaseResponsDTO
    {
        public int GuestId { get; set; }
        public string GuestEmail { get; set; }
        public int TemplateId_Fk { get; set; }
        public string TemplateTitle { get; set; }
        public DateTime PurchaseTime { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
