using System.Security.Claims;
using LendMe.Shared.Application.Chats.Commands.Create;
using LendMe.Shared.Application.Chats.Commands.Update;
using LendMe.Shared.Application.Chats.Dto.Request;
using LendMe.Shared.Application.Chats.Dto.Response;
using LendMe.Shared.Application.Chats.Queries;
using LendMe.Shared.Core.Entities.ChatService;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LendMe.Shared.Web.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
//[Authorize]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ChatController> _logger;

    public ChatController(IMediator mediator, ILogger<ChatController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("{chatId}/messages")]
    [ProducesResponseType(typeof(SendMessageResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<SendMessageResponse>> SendMessage(
        Guid chatId, 
        [FromBody] SendMessageRequest request)
    {
        try
        {
            var command = new SendMessageCommand
            {
                ChatId = chatId,
                SenderId = GetCurrentUserId(),
                Content = new MessageContent()
                {
                    Text = request.Content
                    
                },
                Type = request.Type
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetChatMessages), new { chatId = chatId }, result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid("Access denied to this chat");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message to chat {ChatId}", chatId);
            return StatusCode(500, "Failed to send message");
        }
    }

    [HttpPut("messages/{messageId}")]
    [ProducesResponseType(typeof(UpdateMessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateMessageResponse>> UpdateMessage(
        Guid messageId, 
        [FromBody] UpdateMessageRequest request)
    {
        try
        {
            var command = new UpdateMessageCommand
            {
                MessageId = messageId,
                UserId = GetCurrentUserId(),
                Content = request.Content
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("not found"))
        {
            return NotFound("Message not found");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating message {MessageId}", messageId);
            return StatusCode(500, "Failed to update message");
        }
    }

    [HttpGet("{chatId}/messages")]
    [ProducesResponseType(typeof(GetChatMessagesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<GetChatMessagesResponse>> GetChatMessages(
        Guid chatId, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 50)
    {
        try
        {
            var query = new GetChatMessagesQuery
            {
                ChatId = chatId,
                UserId = GetCurrentUserId(),
                Page = page,
                PageSize = Math.Min(pageSize, 100) // Ограничиваем размер страницы
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid("Access denied to this chat");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting messages for chat {ChatId}", chatId);
            return StatusCode(500, "Failed to get messages");
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                         User.FindFirst("sub")?.Value ??
                         User.FindFirst("id")?.Value;
        
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }
}