namespace BlueBerry24.Domain.Entities.AuthEntities
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public bool RequireHttps { get; set; }
    }
}
