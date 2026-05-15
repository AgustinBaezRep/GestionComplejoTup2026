namespace GestionComplejo.Application.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
