namespace CC_Karriarpartner.DTOs.AdminPanelDtos
{
    public class PurchaseResponseDto
    {
        public int PurchaseId { get; set; }
        public DateTime BuyDate { get; set; }
        public decimal Price { get; set; }
        public string TransactionId { get; set; }

        public UserPurchaseDto User { get; set; }

        // Items in this purchase
        public List<PurchaseItemDto> Items { get; set; }
    }
}
