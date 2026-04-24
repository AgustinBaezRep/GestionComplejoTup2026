namespace GestionComplejo.Application.Responses
{
    public class ServicioResponse
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public double CostoAdicional { get; set; }
    }
}
