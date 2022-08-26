using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Stock.Application.Common.Dependency.DataAccess.Repositories.Common;
using Stock.Application.Common.Mapping;
using Stock.Domain.Common;
using Stock.Infrastructure.Common.Extensions;
using Stock.Infrastructure.Persistence.Context;

namespace Stock.Infrastructure.ApplicationDependencies.DataAccess.Repositories.Common;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
{
    protected DbSet<TEntity> _set;
    protected readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    protected abstract IQueryable<TEntity> BaseQuery { get; }

    public RepositoryBase(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _set = context.Set<TEntity>();
        _mapper = mapper;
    }
    
    public async Task<TEntity> GetByIdAsync(string id)
        => await BaseQuery.SingleOrDefaultAsync(e => e.Id == id);

    public async Task<IEnumerable<TEntity>> GetFiltered(Expression<Func<TEntity, bool>> filter, bool readOnly = false)
        => await (readOnly ? BaseQuery.AsNoTracking() : BaseQuery).Where(filter).ToListAsync();

    public virtual void Add(TEntity entity)
        => _set.Add(entity);

    public virtual void AddRange(IEnumerable<TEntity> entities)
        => _set.AddRange(entities);

    public virtual void Remove(TEntity entity)
        => _set.Remove(entity);

    public abstract void RemoveRange(IEnumerable<TEntity> entitiesToDelete);

    public virtual void StartTracking(TEntity entity)
        => _set.Update(entity);

    public virtual async Task<TDto> GetProjectedAsync<TDto>(string id, bool readOnly = false) where TDto : IMapFrom<TEntity>
        => await (readOnly ? BaseQuery.AsNoTracking() : BaseQuery)
            .Where(x => x.Id == id)
            .ProjectTo<TDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();

    public virtual async Task<IPaginatedResponseModel<TDto>> GetProjectedPaginatedListAsync<TDto>(PaginatedQueryModel<TDto> model, Expression<Func<TEntity, bool>>? additionalFilter = null,
        bool readOnly = false) where TDto : IMapFrom<TEntity>
    {
        var query = readOnly ? BaseQuery.AsNoTracking() : BaseQuery;

        if (additionalFilter != null)
        {
            query = query.Where(additionalFilter);
        }

        IQueryable<TDto> filteredDtoQuery = default;
        try
        {
            filteredDtoQuery = query
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ApplyFilter(model.Filter);
        }
        catch(FormatException fe)
        {
            model.ThrowFilterIncorrectException(fe.InnerException);
        }

        var totalRowCount = await filteredDtoQuery.CountAsync();

        IEnumerable<TDto> resultPage = default;
        try
        {
            resultPage = await filteredDtoQuery
                .ApplyOrder(model.OrderBy)
                .ApplyPaging(model.PageSize, model.PageIndex)
                .ToListAsync();
        }
        catch(FormatException fe)
        {
            model.ThrowOrderByIncorrectException(fe.InnerException);
        }

        return new PaginatedResponseModel<TDto>(model, totalRowCount, resultPage);
    }
}
