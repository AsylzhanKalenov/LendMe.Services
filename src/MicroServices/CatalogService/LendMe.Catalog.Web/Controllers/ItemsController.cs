using Asp.Versioning;
using LendMe.Catalog.Application.Queries;
using LendMe.Catalog.Application.Queries.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LendMe.Catalog.Web.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Produces("application/json")]
[ApiVersion("1")]
public class ItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    // GET
    [HttpGet("categories")]
    public async Task<ActionResult<List<CategoryDto>>> CategoryList(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCategoriesQuery(), cancellationToken);
        
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetItemsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetItems([FromQuery] GetItemsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}