﻿using Lendme.Core.Entities.ReviewService;
using Lendme.Core.Interfaces;
using Lendme.Infrastructure.MongoPersistence.Documents.Mapper;
using Lendme.Infrastructure.MongoPersistence.Documents.ReviewDocs;
using MongoDB.Driver;

namespace Lendme.Infrastructure.Implementations;

public class ReviewRepository : IReviewRepository
{
    private readonly IMongoCollection<ReviewDocument> _collection;

    public ReviewRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<ReviewDocument>("products");
    }
    
    public async Task<List<Review>> GetReviewsByItemAsync(Guid itemId)
    {
        var documents = await _collection.Find(d => d.ItemId == itemId).ToListAsync();
        return documents.ToEntityList();
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