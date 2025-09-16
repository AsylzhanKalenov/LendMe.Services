using Asp.Versioning;
using Lendme.Application.Catalog.Commands;
using Lendme.Application.Catalog.Queries;
using Lendme.Application.Catalog.Queries.Dto;
using Lendme.Core.Entities.Catalog;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lendme.Web.Controllers;

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

    [HttpPost]
    [ProducesResponseType(typeof(CreateItemResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateItem([FromBody] CreateItemCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetItems), new { id = result.Id }, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetItemsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetItems([FromQuery] GetItemsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}