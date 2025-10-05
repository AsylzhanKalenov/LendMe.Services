using Asp.Versioning;
using AutoMapper;
using LendMe.Catalog.Application.Commands.Item.Create;
using LendMe.Catalog.Application.Commands.Item.Update;
using LendMe.Catalog.Application.Dto;
using LendMe.Catalog.Application.Dto.Create;
using LendMe.Catalog.Application.Dto.Update;
using LendMe.Catalog.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LendMe.Catalog.Web.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Produces("application/json")]
[ApiVersion("1")]
public class ItemsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ItemsController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
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

    [HttpPost]
    [ProducesResponseType(typeof(CreateItemResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddItem(CreateItemDto item)
    {
        var result = await _mediator.Send(_mapper.Map<CreateItemCommand>(item));
        return Ok(result);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateItemResponse>> Update(
        Guid id,
        [FromBody] UpdateItemDto item,
        CancellationToken cancellationToken)
    {
        item.Id = id;
        var result = await _mediator.Send(_mapper.Map<UpdateItemCommand>(item), cancellationToken);
        return Ok(result);
    }
}