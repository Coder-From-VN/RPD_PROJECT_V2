namespace RPD_WEB.Models.Auth
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; }
    }
}
