using CC_Karriarpartner.DTOs.AdminPanelDtos;

namespace CC_Karriarpartner.Services.IAdminServices
{
    public interface IAdminPanel
    {
        Task<List<PurchaseResponseDto>> GetAllPurchases();

    }
}
