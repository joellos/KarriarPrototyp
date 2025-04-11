using CC_Karriarpartner.Services.IUserServices;
using System.Net;
using System.Net.Mail;

namespace CC_Karriarpartner.Services.UserServices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smptSettings = _configuration.GetSection("SmtpSettings");
            using var client = new SmtpClient(smptSettings["Host"])
            {
                Port = int.Parse(smptSettings["Port"]),
                Credentials = new NetworkCredential(smptSettings["Username"], smptSettings["Password"]),
                EnableSsl = bool.Parse(smptSettings["EnableSsl"])
            };

            var message = new MailMessage
            {
                From = new MailAddress(smptSettings["SenderEmail"], smptSettings["SenderName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(to);

            await client.SendMailAsync(message);
        }

        public async Task SendVerificationEmailAsync(string email, string verificationToken, string username)
        {
            string baseUrl = _configuration["ApplicationUrl"];
            string verificationUrl = $"{baseUrl}/api/verify?email={WebUtility.UrlEncode(email)}&token={WebUtility.UrlEncode(verificationToken)}";

            string subject = "Bekräfta din e-postadress";
            string body = $@"
                <html>
                <body>
                    <h2>Välkommen till Karriär Partner, {username!}</h2>
                    <p>Tack för att du registrerade dig. Klicka på länken nedan för att bekräfta din e-postadress:</p>
                    <p><a href='{verificationUrl}'>Bekräfta e-post</a></p>
                    <p>Om du inte har registrerat ett konto kan du ignorera detta mejl.</p>
                </body>
                </html>";

            await SendEmailAsync(email, subject, body);

        }

        public async Task SendPasswordResetEmailAsync(string email, string resetToken, string userName)
        {
            string baseUrl = _configuration["ApplicationUrl"];
            // This should point to your /reset-password endpoint, not directly to HTML
            string resetUrl = $"{baseUrl}/reset-password?email={WebUtility.UrlEncode(email)}&token={WebUtility.UrlEncode(resetToken)}";

            string subject = "Reset Your Password";
            string body = $@"
        <html>
        <body>
            <h2>Password Reset Request</h2>
            <p>Hello {userName},</p>
            <p>We received a request to reset your password. Please click the link below to set a new password:</p>
            <p><a href='{resetUrl}'>Reset Password</a></p>
            <p>This link will expire in 24 hours.</p>
            <p>If you didn't request this, please ignore this email.</p>
        </body>
        </html>";

            await SendEmailAsync(email, subject, body);
        }

    }
}
