using AutoMapper;
using Stock.Application.Common.Dependency.DataAccess.Repositories;
using Stock.Domain.Partners;
using Stock.Infrastructure.ApplicationDependencies.DataAccess.Repositories.Common;
using Stock.Infrastructure.Persistence.Context;

namespace Stock.Infrastructure.ApplicationDependencies.DataAccess.Repositories;

internal class PartnerRepository : RepositoryBase<Partner>, IPartnerRepository
{
    protected override IQueryable<Partner> BaseQuery
        => _context.Partners;

    public PartnerRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    { }

    public override void Remove(Partner entityToDelete)
        =>  _context.Remove(entityToDelete);

    public override void RemoveRange(IEnumerable<Partner> entitiesToDelete)
    {
        foreach (var e in entitiesToDelete)
            Remove(e);
    }
}