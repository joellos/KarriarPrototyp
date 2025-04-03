using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.DTOs.AdminPanelDtos
{
    public class UserPurchaseDto
    {
        public int UserId { get; set; }
        [Required]
        [MaxLength(35)]
        public string Namn { get; set; }

        [Required]
        [MaxLength(35)]
        public string Efternamn { get; set; }

        [EmailAddress]
        [Required]
        [MaxLength(50)]
        public string MejlAdress { get; set; }
    }
}
