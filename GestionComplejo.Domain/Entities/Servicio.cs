namespace GestionComplejo.Domain.Entities
{
    public class Servicio : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public double CostoAdicional { get; set; }
        public ICollection<Cancha> Canchas { get; set; } = new List<Cancha>();
    }
}
