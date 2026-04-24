namespace GestionComplejo.Domain.Entities
{
    public abstract class Usuario : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }
}
