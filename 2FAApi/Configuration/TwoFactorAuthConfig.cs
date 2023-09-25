namespace _2FAApi.Configuration
{
    public class TwoFactorAuthConfig
    {
        public int CodeLifetimeMinutes { get; set; }
        public int MaxConcurrentCodesPerPhone { get; set; }
    }
}
