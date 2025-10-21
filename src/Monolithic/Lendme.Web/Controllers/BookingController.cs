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
    private readonly ILogger<BookingController> _logger;

    public BookingController(IMediator mediator, ILogger<BookingController> logger)
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
            RentId = request.RentId,
            ItemId = request.ItemId,
            RenterId = request.RenterId,
            OwnerId = request.OwnerId
        }, cancellationToken);
        
        return Ok(result);
    }

    [HttpPost("UploadReceipt")]
    public async Task<ActionResult<UploadReceiptResponse>> UploadReceipt(string id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new UploadReceiptCommand()
        {
            BookingId = Guid.Parse(id)
        }, cancellationToken));
    }
    
    [HttpPost("ConfirmOwner")]
    public async Task<ActionResult<ConfirmOwnerResponse>> ConfirmOwner(string id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new ConfirmOwnerCommand()
        {
            BookingId = Guid.Parse(id)
        }, cancellationToken));
    }
    
    [HttpPost("ConfirmRenter")]
    public async Task<ActionResult<ConfirmRenterResponse>> ConfirmRenter(string id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new ConfirmRenterCommand()
        {
            BookingId = Guid.Parse(id)
        }, cancellationToken));
    }
    
    [HttpPost("RenterReturnItem")]
    public async Task<ActionResult<RenterReturnItemResponse>> RenterReturnItem(string id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new RenterReturnItemCommand()
        {
            BookingId = Guid.Parse(id)
        }, cancellationToken));
    }
    
    [HttpPost("OwnerReturnItemConfirm")]
    public async Task<ActionResult<OwnerReturnItemConfirmResponse>> OwnerReturnItemConfirm(string id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new OwnerReturnItemConfirmCommand()
        {
            BookingId = Guid.Parse(id)
        }, cancellationToken));
    }
}