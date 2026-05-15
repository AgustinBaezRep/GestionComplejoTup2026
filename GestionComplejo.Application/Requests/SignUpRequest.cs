namespace GestionComplejo.Application.Requests
{
    public class SignUpRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty; // "Cliente" | "Admin"
        public string? Cargo { get; set; } // Requerido si Rol == "Admin"
    }
}
