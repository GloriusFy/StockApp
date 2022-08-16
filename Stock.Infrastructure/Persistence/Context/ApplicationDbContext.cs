using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Application.Common.Dependency.Services;
using Stock.Domain.Common;
using Stock.Domain.Common.ValueObjects.Mass;
using Stock.Domain.Common.ValueObjects.Money;
using Stock.Domain.Partners;
using Stock.Domain.Products;
using Stock.Domain.Transactions;
using Stock.Infrastructure.Identity.Models;

namespace Stock.Infrastructure.Persistence.Context;

public sealed class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Partner> Partners => Set<Partner>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    private readonly ICurrentUserService _currentUser;
    private readonly IDateTime _dateTime;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, 
        ICurrentUserService currentUser, IDateTime dateTime) : base(options)
    {
        _currentUser = currentUser;
        _dateTime = dateTime;
        ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;

    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }
    
    
    public override int SaveChanges()
        => SaveChanges(acceptAllChangesOnSuccess: true);

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyEntityOverrides();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyEntityOverrides();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        ConfigureValueObjects(builder);
        ConfigureDecimalPrecision(builder);
        ConfigureSoftDeleteFilter(builder);

        base.OnModelCreating(builder);
    }

    private void ConfigureDecimalPrecision(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var decimalProperty in entityType.GetProperties()
                         .Where(x => x.ClrType == typeof(decimal)))
            {
                decimalProperty.SetPrecision(18);
                decimalProperty.SetScale(4);
            }
        }
    }

    private void ConfigureSoftDeleteFilter(ModelBuilder builder)
    {
        foreach (var softDeletableTypeBuilder in builder.Model.GetEntityTypes()
                     .Where(x => typeof(ISoftDeletableEntity).IsAssignableFrom(x.ClrType)))
        {
            var parameter = Expression.Parameter(softDeletableTypeBuilder.ClrType, "p");

            softDeletableTypeBuilder.SetQueryFilter(
                Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, nameof(ISoftDeletableEntity.DeletedAt)),
                        Expression.Constant(null)),
                    parameter)
            );
        }
    }

    private void ConfigureValueObjects(ModelBuilder builder)
    {
        builder.Owned(typeof(Money));
        builder.Owned(typeof(Currency));
        builder.Owned(typeof(Address));
        builder.Owned(typeof(Mass));
        builder.Owned(typeof(MassUnit));

        builder.Entity<Product>().OwnsOne(p => p.Mass, StoreMassUnitAsSymbol);

        builder.Entity<Product>().OwnsOne(p => p.Price, StoreCurrencyAsCode);
        builder.Entity<Transaction>().OwnsOne(p => p.Total, StoreCurrencyAsCode);
        builder.Entity<TransactionLine>().OwnsOne(p => p.UnitPrice, StoreCurrencyAsCode);

        static void StoreCurrencyAsCode<T>(OwnedNavigationBuilder<T, Money> onb) where T : class
            => onb.Property(m => m.Currency)
                .HasConversion(
                    c => c.Code,
                    c => Currency.FromCode(c))
                .HasMaxLength(3);

        static void StoreMassUnitAsSymbol<T>(OwnedNavigationBuilder<T, Mass> onb) where T : class
            => onb.Property(m => m.Unit)
                .HasConversion(
                    u => u.Symbol,
                    s => MassUnit.FromSymbol(s))
                .HasMaxLength(3);
    }

    private void ApplyEntityOverrides()
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property(nameof(IAuditableEntity.CreatedBy)).CurrentValue = _currentUser.UserId;
                    entry.Property(nameof(IAuditableEntity.CreatedAt)).CurrentValue = _dateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Property(nameof(IAuditableEntity.LastModifiedBy)).CurrentValue = _currentUser.UserId;
                    entry.Property(nameof(IAuditableEntity.LastModifiedAt)).CurrentValue = _dateTime.Now;
                    break;
            }
        }

        foreach (var entry in ChangeTracker.Entries<ISoftDeletableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    entry.Property(nameof(ISoftDeletableEntity.DeletedBy)).CurrentValue = _currentUser.UserId;
                    entry.Property(nameof(ISoftDeletableEntity.DeletedAt)).CurrentValue = _dateTime.Now;
                    break;
            }
        }
    }
}