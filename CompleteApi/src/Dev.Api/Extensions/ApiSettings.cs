namespace Dev.Api.Extensions
{
    public class ApiSettings
    {
        public string Secret { get; set; }

        public int ExpirationHours { get; set; }

        public string Issuer { get; set; }

        public string ValidTo { get; set; }
    }
}
