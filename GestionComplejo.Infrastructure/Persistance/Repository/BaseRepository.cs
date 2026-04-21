using GestionComplejo.Application.Abstractions.Infrastructure;
using GestionComplejo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionComplejo.Infrastructure.Persistance.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly GestionComplejoDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(GestionComplejoDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual List<T> GetAll()
        {
            return _dbSet.Where(x => !x.IsDeleted).ToList();
        }

        public virtual T? GetById(Guid id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        }

        public virtual T Add(T entity)
        {
            entity.UpdatedDateTime = DateTime.UtcNow;

            _dbSet.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual void Update(T entity)
        {
            entity.UpdatedDateTime = DateTime.UtcNow;

            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public virtual void Delete(Guid id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.DeletedDateTime = DateTime.UtcNow;
                entity.UpdatedDateTime = DateTime.UtcNow;

                _dbSet.Update(entity);
                _context.SaveChanges();
            }
        }
    }
}
