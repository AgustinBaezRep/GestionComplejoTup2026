namespace GestionComplejo.Application.Abstractions
{
    public interface IClimaService
    {
        Task<bool> EsLluviosoAsync(DateTime fecha);
    }
}
