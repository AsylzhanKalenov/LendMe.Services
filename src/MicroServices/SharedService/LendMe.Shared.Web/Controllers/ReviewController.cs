using Asp.Versioning;
using LendMe.Shared.Application.Reviews.Commands.Create;
using LendMe.Shared.Application.Reviews.Dto;
using LendMe.Shared.Application.Reviews.Dto.Response;
using LendMe.Shared.Application.Reviews.Queries;
using LendMe.Shared.Core.Entities.ReviewService;
using LendMe.Shared.Core.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LendMe.Shared.Web.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Produces("application/json")]
[ApiVersion("1.0")]
public class ReviewController: ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    // GET
    [HttpGet]
    public async Task<ActionResult<GetItemReviewsResponse>> Index(string itemId, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetItemReviewsQuery()
        {
            ItemId = itemId
        }, cancellationToken);
    }

    [HttpPost]
    public async Task<ActionResult<CreateReviewResponse>> Create([FromBody] ReviewDto review, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new CreateReviewByItemIdCommand()
        {
            CreateReview = review
        }, cancellationToken);
    }

}