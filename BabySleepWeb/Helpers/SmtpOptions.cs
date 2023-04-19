namespace BabySleepWeb.Helpers
{
    public class SmtpOptions
    {
        public const string Smtp = "Smtp";

        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}
