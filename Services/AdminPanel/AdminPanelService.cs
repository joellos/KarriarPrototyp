using CC_Karriarpartner.Data;
using CC_Karriarpartner.DTOs.AdminPanelDtos;
using CC_Karriarpartner.Models;
using CC_Karriarpartner.Services.IAdminServices;
using Microsoft.EntityFrameworkCore;

namespace CC_Karriarpartner.Services.AdminPanel
{
    public class AdminPanelService : IAdminPanel
    {
        private readonly KarriarPartnerDBContext context;

        public AdminPanelService(KarriarPartnerDBContext _context)
        {
            context = _context;
        }

        public async Task<(List<PurchaseResponseDto> Purchases, int TotalCount)> GetAllPurchases(int page = 1, int pageSize = 10)//adminPanel endrpoint to get all pruchases 
        {
            page = Math.Max(1, page); //max to avoid errors from user, it will take first page if input 0 
            pageSize = Math.Max(1, pageSize);

            var totalCount = await context.Purchases.CountAsync();

            var purchases = await context.Purchases
                .OrderByDescending(p => p.BuyDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                  .Include(p => p.User)
                  .Include(p => p.PurchaseItems)
                  .ThenInclude(pi => pi.Course)
                  .Include(p => p.PurchaseItems)
                  .ThenInclude(pi => pi.Template)
                  .ToListAsync();

            var result = purchases.Select(p => new PurchaseResponseDto
            {
                PurchaseId = p.PurchaseId,
                BuyDate = p.BuyDate,
                Price = p.Price,
                TransactionId = p.TransactionId,
                User = new UserPurchaseDto
                {
                    UserId = p.User.UserId,
                    Namn = p.User.Name,
                    Efternamn = p.User.LastName,
                    MejlAdress = p.User.Email
                },
                Items = p.PurchaseItems.Select(pi => new PurchaseItemDto
                {
                    PurchaseItemId = pi.PurchaseItemId,
                    ItemType = pi.Course != null ? "Course" : "Template",
                    Title = pi.Course != null ? pi.Course.Title : pi.Template.Title,
                    Price = pi.Course != null ? pi.Course.Price : pi.Template.Price
                })
                .ToList()
            }).ToList();
            return (result, totalCount);
        }
    }

}
