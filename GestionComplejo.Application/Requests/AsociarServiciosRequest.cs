namespace GestionComplejo.Application.Requests
{
    public class AsociarServiciosRequest
    {
        public List<Guid> ServicioIds { get; set; } = new();
    }
}
