using LendMe.Catalog.Core.Entity;

namespace LendMe.Catalog.Core.Interfaces.Repository;

public interface IRentRepository
{
    Task<Rent> AddAsync(Rent rent, CancellationToken cancellationToken);
    Rent Update(Rent rent);
    Task<Rent?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}