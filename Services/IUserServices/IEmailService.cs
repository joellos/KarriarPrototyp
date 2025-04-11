namespace CC_Karriarpartner.Services.IUserServices
{
    public interface IEmailService
    {
        Task SendEmailAsync (string to, string subject, string body);
        Task SendVerificationEmailAsync(string email, string verificationToken, string username);

        Task SendPasswordResetEmailAsync(string email, string resetToken, string userName);
    }
}
