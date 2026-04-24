namespace GestionComplejo.Domain.Entities
{
    public class Admin : Usuario
    {
        public string Cargo { get; set; } = string.Empty;
    }
}
