using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.DTOs.PasswordDtos
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
