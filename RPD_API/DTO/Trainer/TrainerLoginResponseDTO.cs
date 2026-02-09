namespace RPD_API.DTO.Trainer
{
    public class TrainerLoginResponseDTO
    {
        public string AccessToken { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }

        public string RefreshToken { get; set; } = null!;
    }
}
