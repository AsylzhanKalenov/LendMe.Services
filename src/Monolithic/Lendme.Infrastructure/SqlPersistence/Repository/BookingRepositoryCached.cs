using System.Text.Json;
using Lendme.Core.Entities.Booking;
using Lendme.Core.Interfaces.Repositories.BookingRepositories;
using Microsoft.Extensions.Caching.Distributed;

namespace Lendme.Infrastructure.SqlPersistence.Repository;

public sealed class BookingRepositoryCached : IBookingRepository
{
    private readonly IBookingRepository _inner;
    private readonly IDistributedCache _cache;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly DistributedCacheEntryOptions _cacheOptions;

    public BookingRepositoryCached(
        IBookingRepository inner,
        IDistributedCache cache)
    {
        _inner = inner;
        _cache = cache;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            // ReferenceHandler = ReferenceHandler.IgnoreCycles // при необходимости
        };
        _cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
        };
    }

    private static string Key(Guid id) => $"booking:{id}";

    public async Task<Booking> AddBookingAsync(Booking booking, CancellationToken cancellationToken)
    {
        var created = await _inner.AddBookingAsync(booking, cancellationToken);
        var payload = JsonSerializer.SerializeToUtf8Bytes(created, _jsonOptions);
        await _cache.SetAsync(Key(created.Id), payload, _cacheOptions, cancellationToken);
        return created;
    }

    public async Task<Booking?> GetBookingByIdAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        var key = Key(bookingId);
        var cached = await _cache.GetAsync(key, cancellationToken);
        if (cached is { Length: > 0 })
        {
            return JsonSerializer.Deserialize<Booking>(cached, _jsonOptions);
        }

        var fromDb = await _inner.GetBookingByIdAsync(bookingId, cancellationToken);
        if (fromDb is not null)
        {
            var payload = JsonSerializer.SerializeToUtf8Bytes(fromDb, _jsonOptions);
            await _cache.SetAsync(key, payload, _cacheOptions, cancellationToken);
        }
        return fromDb;
    }

    public async Task<Booking> UpdateBookingAsync(Booking booking, CancellationToken cancellationToken)
    {
        var updated = await _inner.UpdateBookingAsync(booking, cancellationToken);
        var payload = JsonSerializer.SerializeToUtf8Bytes(updated, _jsonOptions);
        await _cache.SetAsync(Key(updated.Id), payload, _cacheOptions, cancellationToken);
        return updated;
    }

    public async Task DeleteBookingAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        await _inner.DeleteBookingAsync(bookingId, cancellationToken);
        await _cache.RemoveAsync(Key(bookingId), cancellationToken);
    }

    public Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        => _inner.SaveChangesAsync(cancellationToken);
}