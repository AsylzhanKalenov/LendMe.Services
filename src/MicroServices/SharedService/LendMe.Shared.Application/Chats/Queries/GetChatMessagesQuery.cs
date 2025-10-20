using LendMe.Shared.Application.Chats.Dto.Response;
using LendMe.Shared.Application.Interfaces.ChatServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LendMe.Shared.Application.Chats.Queries;

public class GetChatMessagesQuery : IRequest<GetChatMessagesResponse>
{
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;

    public class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, GetChatMessagesResponse>
    {
        private readonly IChatService _chatService;
        private readonly ILogger<GetChatMessagesQueryHandler> _logger;

        public GetChatMessagesQueryHandler(IChatService chatService, ILogger<GetChatMessagesQueryHandler> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        public async Task<GetChatMessagesResponse> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var hasAccess = await _chatService.HasUserAccessToChatAsync(request.ChatId, request.UserId);
                if (!hasAccess)
                {
                    throw new UnauthorizedAccessException($"User {request.UserId} does not have access to chat {request.ChatId}");
                }

                var messages = await _chatService.GetChatMessagesAsync(request.ChatId, request.Page, request.PageSize);
                var totalCount = await _chatService.GetChatMessageCountAsync(request.ChatId);

                var hasNextPage = (request.Page * request.PageSize) < totalCount;

                return new GetChatMessagesResponse
                {
                    Messages = messages,
                    TotalCount = totalCount,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    HasNextPage = hasNextPage
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get messages for chat {ChatId}", request.ChatId);
                throw;
            }
        }
    }
}