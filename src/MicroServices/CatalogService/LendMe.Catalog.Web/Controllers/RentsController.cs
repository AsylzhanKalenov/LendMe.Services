using Asp.Versioning;
using AutoMapper;
using LendMe.Catalog.Application.Commands.Rents.Create;
using LendMe.Catalog.Application.Dto.Create;
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
    
    [HttpPost]
    [ProducesResponseType(typeof(CreateRentResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddRent(CreateRentDto rent)
    {
        var result = await _mediator.Send(_mapper.Map<CreateRentCommand>(rent));
        return Ok(result);
    }
}