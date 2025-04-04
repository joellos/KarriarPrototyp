using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(35)]
        public string Name { get; set; }

        [Required]
        [MaxLength(35)]
        public string LastName { get; set; }

        [EmailAddress]
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Phone]
        [Required]
        [MinLength(9)] // Utan landskod
        [MaxLength(15)] // Med landskod
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        //Skyddas i Apiet, Hasha lösenord med BCrypt eller ASP.NET Identity
        public string Password { get; set; }

        public bool Verified { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; }

        public string? PasswordResetToken { get; set; }

        public DateTime? PasswordResetExpire { get; set; }

        [Url]
        public string? ProfileImageUrl { get; set; }

        public DateTime? LastLogin { get; set; }
        public string? EmailVerification { get; set; }
        // tokens for authentication
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }

        public string Role { get; set; } = string.Empty;

        public virtual List<UserSubscriptions> Subscriptions { get; set; }
        public virtual List<Purchase> Purchases { get; set; }
        public virtual List<Certificate> Certificates { get; set; }
        public virtual List<CourseReview> Reviews { get; set; }
    }
}
