using LendMe.Shared.Core.Entities.ReviewService;
using LendMe.Shared.Core.Repositories;
using LendMe.Shared.Infrastructure.MongoPersistence.Documents.ReviewDocs;
using LendMe.Shared.Infrastructure.MongoPersistence.Mapper;
using MongoDB.Driver;

namespace LendMe.Shared.Infrastructure.MongoPersistence.Implementations;

public class ReviewRepository : IReviewRepository
{
    private readonly IMongoCollection<ReviewDocument> _collection;

    public ReviewRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<ReviewDocument>("LendmeReviews");
    }
    
    public async Task<List<Review>> GetReviewsByItemAsync(string itemId)
    {
        try
        {
            Guid.TryParse(itemId.ToString(), out var id);
            var documents = await _collection.Find(d => d.ItemId == id).ToListAsync();
            return documents.ToEntityList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Review?> GetReviewByIdAsync(Guid reviewId)
    {
        var document = await _collection
            .Find(d => d.Id == reviewId)
            .FirstOrDefaultAsync();
        
        return document?.ToEntity();
    }

    public async Task<Review?> CreateReviewAsync(Review review)
    {
        var document = review.ToDocument();
        await _collection.InsertOneAsync(document);
        return document.ToEntity();
    }

    public async Task<Review?> UpdateReviewAsync(Review review)
    {
        var document = review.ToDocument();
        await _collection.ReplaceOneAsync(d => d.Id == review.Id, document);
        return document.ToEntity();

    }

    public async Task DeleteReviewAsync(Guid reviewId)
    {
        await _collection.DeleteOneAsync(d => d.Id == reviewId);
    }
}