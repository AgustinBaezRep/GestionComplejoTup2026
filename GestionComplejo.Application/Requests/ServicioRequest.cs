namespace GestionComplejo.Application.Requests
{
    public class ServicioRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public double CostoAdicional { get; set; }
    }
}
