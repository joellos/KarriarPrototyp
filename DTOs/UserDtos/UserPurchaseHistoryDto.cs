namespace CC_Karriarpartner.DTOs.UserDtos
{
    public class UserPurchaseHistoryDto
    {
        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Price { get; set; }
        public List<UserPurchaseItemDto> Items { get; set; } = new List<UserPurchaseItemDto>();

    }
}
