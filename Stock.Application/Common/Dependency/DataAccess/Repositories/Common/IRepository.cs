using System.Linq.Expressions;
using Stock.Application.Common.Mapping;
using Stock.Domain.Common;

namespace Stock.Application.Common.Dependency.DataAccess.Repositories.Common;

public interface IRepository<TEntity> where TEntity : IBaseEntity
{
    Task<TEntity> GetByIdAsync(string id);

    Task<IEnumerable<TEntity>> GetFiltered(Expression<Func<TEntity, bool>> filter, bool readOnly = false);

    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);

    void StartTracking(TEntity entity);

    Task<TDto> GetProjectedAsync<TDto>(string id, bool readOnly = false) where TDto : IMapFrom<TEntity>;

    Task<IPaginatedResponseModel<TDto>> GetProjectedPaginatedListAsync<TDto>(PaginatedQueryModel<TDto> model,
        Expression<Func<TEntity, bool>>? additionalFilter = null, bool readOnly = false) where TDto : IMapFrom<TEntity>;
}