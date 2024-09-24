using Student_Management.Models.Domain.Base;

namespace Student_Management.Sevices
{
    public interface IEntityService<TEntity> :IDisposable where TEntity : EntityBase
    {
        Task<TEntity> GetByIdAsync(int id);

        Task<TEntity> GetByKeyAsync(params object[] keys);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetByParamAsync();

        Task<TEntity> SaveAsync(TEntity entitytoSave);

        bool ExistsAsync(int id);

        bool ExistsAsync(params object[] keys);

    }
}
