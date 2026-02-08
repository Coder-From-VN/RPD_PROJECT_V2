namespace RPD_API.DTO.Erro
{
    public class ErrorResponseDTO
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = default!;
        public string TraceId { get; set; } = default!;
    }
}
