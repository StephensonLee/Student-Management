using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using Student_Management.Data;
using Student_Management.Models;
using Student_Management.Models.Domain;
using Student_Management.Models.Domain.Base;
using State = Student_Management.Models.Domain.State;


namespace Student_Management.Sevices
{
    public class EntityService<TEntity> : IEntityService<TEntity> where TEntity : EntityBase
    {
        private readonly StudentManagementContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public EntityService(StudentManagementContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(e=>e.Id == id);
        }

        public async Task<bool> ExistsAsync(params object[] keys)
        {
            TEntity entity = await _dbSet.FindAsync(keys);
            return (entity!=null);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync(); 
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
         
        public async Task<TEntity> GetByKeyAsync(params object[] keys)
        {
            return await _dbSet.FindAsync(keys);
        }

        public async Task<IEnumerable<TEntity>> GetByParamAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> SaveAsync(TEntity entityToSave)
        {
            if (entityToSave.State == State.Added)
                _dbSet.Add(entityToSave);
            else if (entityToSave.State == State.Deleted)
                _dbSet.Remove(entityToSave);
            else if (entityToSave.State == State.Modified)
                _dbSet.Update(entityToSave);
            
            if (entityToSave.State != State.Unchanged)
            {
                await _context.SaveChangesAsync();
                entityToSave.State = State.Unchanged;
            }
            return entityToSave;
        }

        bool IEntityService<TEntity>.ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        bool IEntityService<TEntity>.ExistsAsync(params object[] keys)
        {
            throw new NotImplementedException();
        }
    }
}
