namespace GestionComplejo.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> GetAll();
        T? GetById(Guid id);
        T Add(T entity);
        void Update(T entity);
        void Delete(Guid id);
    }
}
