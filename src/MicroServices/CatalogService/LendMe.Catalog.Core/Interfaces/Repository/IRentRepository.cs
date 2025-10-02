using LendMe.Catalog.Core.Entity;

namespace LendMe.Catalog.Core.Interfaces.Repository;

public interface IRentRepository
{
    Task<Rent> AddAsync(Rent rent, CancellationToken cancellationToken);
    Task<Rent> UpdateAsync(Rent rent, CancellationToken cancellationToken);
    Task<Rent?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}