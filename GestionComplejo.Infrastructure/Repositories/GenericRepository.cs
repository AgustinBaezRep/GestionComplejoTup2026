using GestionComplejo.Domain.Interfaces;
using GestionComplejo.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionComplejo.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual T? GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public virtual T Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public virtual void Delete(Guid id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}
