using Asp.Versioning;
using AutoMapper;
using LendMe.Catalog.Application.Commands.Rents.Create;
using LendMe.Catalog.Application.Commands.Rents.Update;
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
public class RentsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public RentsController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(GetRentsQuery), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetItems([FromQuery] GetRentsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(CreateRentResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddRent(CreateRentDto rent)
    {
        var result = await _mediator.Send(_mapper.Map<CreateRentCommand>(rent));
        return Ok(result);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateRentResponse>> Update(
        Guid id,
        [FromBody] UpdateRentDto rent,
        CancellationToken cancellationToken)
    {
        rent.Id = id;
        var result = await _mediator.Send(_mapper.Map<UpdateRentCommand>(rent), cancellationToken);
        return Ok(result);
    }
}