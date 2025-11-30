namespace BlueBerry24.Application.Config
{ 
    public class JwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public bool RequireHttps { get; set; }
    }
}
