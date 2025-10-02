using LendMe.Catalog.Core.Entity;
using LendMe.Catalog.Core.Interfaces.Repository;
using LendMe.Catalog.Infrastructure.SqlPersistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LendMe.Catalog.Infrastructure.SqlPersistence.Repositories;

public class RentRepository : IRentRepository
{
    private readonly ApplicationDbContext _context;

    public RentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Rent> AddAsync(Rent rent, CancellationToken cancellationToken)
    {
        await _context.Rents.AddAsync(rent, cancellationToken);
        return rent;
    }

    public async Task<Rent> UpdateAsync(Rent rent, CancellationToken cancellationToken)
    {
        _context.Rents.Update(rent);
        return rent;
    }

    public async Task<Rent?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Rents.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}