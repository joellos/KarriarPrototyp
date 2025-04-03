namespace CC_Karriarpartner.DTOs
{
    public class CreateCertificateDto
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public string CertificateUrl { get; set; }
        public DateTime IssuedAt { get; set; }
    }
}

