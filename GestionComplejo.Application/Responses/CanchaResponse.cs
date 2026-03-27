namespace GestionComplejo.Application.Responses
{
    public class CanchaResponse
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Deporte { get; set; } = string.Empty;
        public double Precio { get; set; }
    }
}
