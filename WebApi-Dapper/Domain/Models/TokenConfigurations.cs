namespace Domain.Models
{
    public class TokenConfigurations
    {
        public string? AccessRole { get; set; }
        public string? SecretJWTKey { get; set; }
        public string? Audience { get; set; }
        public string? Issuer { get; set; }
        public int Seconds { get; set; }
        public string? FinalExpiration { get; set; }
    }
}
