namespace BlueBerry24.Application.Config
{
    public class GmailSettings
    {
        public const string Name = "GmailSettings";
        public string Email { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string DisplayName { get; set; }
    }
}