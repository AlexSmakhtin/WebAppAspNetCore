using Models;

namespace Domain.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        Task<TEntity> GetById(Guid Id, CancellationToken token);
        Task<IReadOnlyCollection<TEntity>> GetAll(CancellationToken token);
        Task Add(TEntity entity, CancellationToken token);
        Task Update(TEntity entity, CancellationToken token);
        Task Delete(TEntity entity, CancellationToken token);
    }
}
