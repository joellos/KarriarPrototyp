namespace CC_Karriarpartner.DTOs.AdminPanelDtos
{
    public class PurchaseItemDto
    {
        public int PurchaseItemId { get; set; }
        public string ItemType { get; set; } // "Course" or "Template"
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
