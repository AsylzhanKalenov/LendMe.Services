using LendMe.Shared.Application.Interfaces.ChatServices;
using LendMe.Shared.Core.Entities.ChatService;
using LendMe.Shared.Infrastructure.MongoPersistence;
using LendMe.Shared.Infrastructure.MongoPersistence.Documents.ChatDocs;
using LendMe.Shared.Infrastructure.MongoPersistence.Mapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LendMe.Shared.Infrastructure.Services;

public class MongoDbChatService : IChatService
{
    private readonly IMongoCollection<ChatDocument> _chatCollection;
    private readonly IMongoCollection<MessageDocument> _messageCollection;
    private readonly ILogger<MongoDbChatService> _logger;

    public MongoDbChatService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, ILogger<MongoDbChatService> logger)
    {
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _chatCollection = database.GetCollection<ChatDocument>("chats");
        _messageCollection = database.GetCollection<MessageDocument>("messages");
        _logger = logger;
        
        // Создаем индексы для оптимизации запросов
        CreateIndexes();
    }

    public async Task<bool> HasUserAccessToChatAsync(Guid chatId, Guid userId)
    {
        try
        {
            var filter = Builders<ChatDocument>.Filter.And(
                Builders<ChatDocument>.Filter.Eq(c => c.Id, chatId),
                Builders<ChatDocument>.Filter.ElemMatch(c => c.Participants,
                    p => p.UserId == userId && p.IsActive)
            );

            var count = await _chatCollection.CountDocumentsAsync(filter);
            return count > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking user access to chat {ChatId} for user {UserId}", chatId, userId);
            return false;
        }
    }

    public async Task<Message> SaveMessageAsync(Message message)
    {
        try
        {
            var messageDocument = MessageMapper.ToDocument(message);
            await _messageCollection.InsertOneAsync(messageDocument);

            // Обновляем счетчики и последнее сообщение в чате
            var chatFilter = Builders<ChatDocument>.Filter.Eq(c => c.Id, message.ChatId);
            var chatUpdate = Builders<ChatDocument>.Update
                .Set(c => c.LastMessageAt, message.SentAt)
                .Inc(c => c.MessageCount, 1);

            await _chatCollection.UpdateOneAsync(chatFilter, chatUpdate);

            // Обновляем статус сообщения
            message.Status = MessageStatus.Sent;
            var messageFilter = Builders<MessageDocument>.Filter.Eq(m => m.Id, message.Id);
            var messageUpdate = Builders<MessageDocument>.Update.Set(m => m.Status, MessageStatus.Sent);
            await _messageCollection.UpdateOneAsync(messageFilter, messageUpdate);

            return message;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving message to chat {ChatId}", message.ChatId);
            message.Status = MessageStatus.Failed;
            throw;
        }
    }

    public async Task<Message?> GetMessageAsync(Guid messageId)
    {
        try
        {
            var filter = Builders<MessageDocument>.Filter.Eq(m => m.Id, messageId);
            var messageDocument = await _messageCollection.Find(filter).FirstOrDefaultAsync();
            return MessageMapper.ToEntity(messageDocument);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting message {MessageId}", messageId);
            throw;
        }
    }

    public async Task<Message> UpdateMessageAsync(Message message)
    {
        try
        {
            var messageDocument = MessageMapper.ToDocument(message);
            var filter = Builders<MessageDocument>.Filter.Eq(m => m.Id, message.Id);
            await _messageCollection.ReplaceOneAsync(filter, messageDocument);
            return message;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating message {MessageId}", message.Id);
            throw;
        }
    }

    public async Task<bool> DeleteMessageAsync(Guid messageId, Guid userId)
    {
        try
        {
            var filter = Builders<MessageDocument>.Filter.And(
                Builders<MessageDocument>.Filter.Eq(m => m.Id, messageId),
                Builders<MessageDocument>.Filter.Eq(m => m.SenderId, userId)
            );

            var update = Builders<MessageDocument>.Update
                .Set(m => m.DeletedAt, DateTime.UtcNow)
                .Set(m => m.EditedAt, DateTime.UtcNow);

            var result = await _messageCollection.UpdateOneAsync(filter, update);
            return result.MatchedCount > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting message {MessageId}", messageId);
            throw;
        }
    }

    public async Task<List<Message>> GetChatMessagesAsync(Guid chatId, int page, int pageSize)
    {
        try
        {
            var filter = Builders<MessageDocument>.Filter.And(
                Builders<MessageDocument>.Filter.Eq(m => m.ChatId, chatId),
                Builders<MessageDocument>.Filter.Eq(m => m.DeletedAt, null)
            );

            var messages = await _messageCollection
                .Find(filter)
                .SortByDescending(m => m.SentAt)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return messages.Select(MessageMapper.ToEntity).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting messages for chat {ChatId}", chatId);
            throw;
        }
    }

    public async Task<int> GetChatMessageCountAsync(Guid chatId)
    {
        try
        {
            var filter = Builders<MessageDocument>.Filter.And(
                Builders<MessageDocument>.Filter.Eq(m => m.ChatId, chatId),
                Builders<MessageDocument>.Filter.Eq(m => m.DeletedAt, null)
            );

            return (int)await _messageCollection.CountDocumentsAsync(filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting message count for chat {ChatId}", chatId);
            throw;
        }
    }

    public async Task MarkMessageAsReadAsync(Guid messageId, Guid userId)
    {
        try
        {
            var filter = Builders<MessageDocument>.Filter.And(
                Builders<MessageDocument>.Filter.Eq(m => m.Id, messageId),
                Builders<MessageDocument>.Filter.Ne(m => m.SenderId, userId)
            );

            var readReceipt = new MessageReadReceiptDocument
            {
                UserId = userId,
                ReadAt = DateTime.UtcNow
            };

            var update = Builders<MessageDocument>.Update.AddToSet(m => m.ReadReceipts, readReceipt);
            await _messageCollection.UpdateOneAsync(filter, update);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking message {MessageId} as read", messageId);
            throw;
        }
    }

    public async Task<Chat?> GetChatAsync(Guid chatId)
    {
        try
        {
            var filter = Builders<ChatDocument>.Filter.Eq(c => c.Id, chatId);
            var chatDocument = await _chatCollection.Find(filter).FirstOrDefaultAsync();
            return ChatMapper.ToEntity(chatDocument);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting chat {ChatId}", chatId);
            throw;
        }
    }

    public async Task<Chat> CreateChatAsync(Chat chat)
    {
        try
        {
            var chatDocument = ChatMapper.ToDocument(chat);
            await _chatCollection.InsertOneAsync(chatDocument);
            return chat;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating chat");
            throw;
        }
    }

    public async Task<List<Chat>> GetUserChatsAsync(Guid userId)
    {
        try
        {
            var filter = Builders<ChatDocument>.Filter.ElemMatch(c => c.Participants,
                p => p.UserId == userId && p.IsActive);

            var chatDocuments = await _chatCollection
                .Find(filter)
                .SortByDescending(c => c.LastMessageAt)
                .ToListAsync();

            return chatDocuments.Select(ChatMapper.ToEntity).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting chats for user {UserId}", userId);
            throw;
        }
    }

    public async Task<List<ChatParticipant>> GetChatParticipantsAsync(Guid chatId)
    {
        try
        {
            var filter = Builders<ChatDocument>.Filter.Eq(c => c.Id, chatId);
            var projection = Builders<ChatDocument>.Projection.Include(c => c.Participants);
            
            var chatDocument = await _chatCollection.Find(filter).Project(projection).FirstOrDefaultAsync();
            var chatDoc = chatDocument?.AsBsonDocument;
            
            if (chatDoc != null && chatDoc.Contains("participants"))
            {
                var participantsArray = chatDoc["participants"].AsBsonArray;
                var participants = participantsArray
                    .Select(p => p.AsBsonDocument)
                    .Where(p => p["isActive"].AsBoolean)
                    .Select(p => new ChatParticipant
                    {
                        Id = Guid.Parse(p["id"].AsString),
                        UserId = Guid.Parse(p["userId"].AsString),
                        Role = (ParticipantRole)p["role"].AsInt32,
                        JoinedAt = p["joinedAt"].ToUniversalTime(),
                        IsActive = p["isActive"].AsBoolean
                    })
                    .ToList();

                return participants;
            }

            return new List<ChatParticipant>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting participants for chat {ChatId}", chatId);
            throw;
        }
    }

    public async Task AddParticipantAsync(Guid chatId, Guid userId, ParticipantRole role = ParticipantRole.Renter)
    {
        try
        {
            var participant = new ChatParticipantDocument
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Role = role,
                JoinedAt = DateTime.UtcNow,
                IsActive = true
            };

            var filter = Builders<ChatDocument>.Filter.Eq(c => c.Id, chatId);
            var update = Builders<ChatDocument>.Update.Push(c => c.Participants, participant);
            
            await _chatCollection.UpdateOneAsync(filter, update);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding participant {UserId} to chat {ChatId}", userId, chatId);
            throw;
        }
    }

    public async Task RemoveParticipantAsync(Guid chatId, Guid userId)
    {
        try
        {
            var filter = Builders<ChatDocument>.Filter.And(
                Builders<ChatDocument>.Filter.Eq(c => c.Id, chatId),
                Builders<ChatDocument>.Filter.ElemMatch(c => c.Participants, p => p.UserId == userId)
            );

            var update = Builders<ChatDocument>.Update.Set("participants.$.isActive", false)
                .Set("participants.$.leftAt", DateTime.UtcNow);

            await _chatCollection.UpdateOneAsync(filter, update);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing participant {UserId} from chat {ChatId}", userId, chatId);
            throw;
        }
    }

    private void CreateIndexes()
    {
        try
        {
            // Индексы для коллекции чатов
            var chatIndexKeys = Builders<ChatDocument>.IndexKeys
                .Ascending("participants.userId")
                .Ascending("participants.isActive");
            _chatCollection.Indexes.CreateOne(new CreateIndexModel<ChatDocument>(chatIndexKeys));

            // Индексы для коллекции сообщений
            var messageIndexKeys = Builders<MessageDocument>.IndexKeys
                .Ascending(m => m.ChatId)
                .Descending(m => m.SentAt);
            _messageCollection.Indexes.CreateOne(new CreateIndexModel<MessageDocument>(messageIndexKeys));

            var senderIndexKeys = Builders<MessageDocument>.IndexKeys.Ascending(m => m.SenderId);
            _messageCollection.Indexes.CreateOne(new CreateIndexModel<MessageDocument>(senderIndexKeys));
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to create MongoDB indexes");
        }
    }
}