using Asp.Versioning;
using Lendme.Core.Entities.ReviewService;
using Lendme.Core.Interfaces;
using Lendme.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lendme.Web.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Produces("application/json")]
[ApiVersion("1.0")]
public class ReviewController: ControllerBase
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewController(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }
    
    // GET
    [HttpGet]
    public ActionResult<List<Review>> Index(string itemId)
    {
        return _reviewRepository.GetReviewsByItemAsync(itemId).Result;
    }

    [HttpPost]
    public ActionResult<Review> Create([FromBody] Review review)
    {
        return _reviewRepository.CreateReviewAsync(review).Result ?? throw new InvalidOperationException();
    }

}