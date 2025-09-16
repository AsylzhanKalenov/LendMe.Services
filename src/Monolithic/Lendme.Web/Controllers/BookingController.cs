using Lendme.Application.Booking.Commands.Create;
using Lendme.Application.Booking.Commands.Update;
using Lendme.Application.Booking.Dto;
using Lendme.Application.Booking.Dto.Request;
using Lendme.Application.Booking.Dto.Response;
using Lendme.Application.Booking.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lendme.Web.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
//[Authorize]
public class BookingController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ChatController> _logger;

    public BookingController(IMediator mediator, ILogger<ChatController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<ActionResult<BookingDto>> GetBooking(string id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBookingQuery()
        {
            Id = Guid.Parse(id)
        }, cancellationToken);
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CreateBookingResponse>> CreateBooking(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateBookingCommand()
        {
            ItemId = request.ItemId,
            RenterId = request.RenterId,
            OwnerId = request.OwnerId
        }, cancellationToken);
        
        return Ok(result);
    }

    [HttpPost("ConfirmBooking")]
    public async Task<ActionResult<ConfirmBookingResponse>> ConfirmBooking(string id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new ConfirmBookingCommand()
        {
            BookingId = Guid.Parse(id)
        }, cancellationToken));
    }
}