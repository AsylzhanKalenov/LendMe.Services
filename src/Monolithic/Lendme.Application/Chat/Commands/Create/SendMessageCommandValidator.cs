using FluentValidation;
using Lendme.Core.Entities.Chat;

namespace Lendme.Application.Chat.Commands.Create;

public class SendMessageCommandValidator: AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.ChatId)
            .NotEmpty()
            .WithMessage("Chat ID is required");

        RuleFor(x => x.SenderId)
            .NotEmpty()
            .WithMessage("Sender ID is required");

        RuleFor(x => x.Content.Text)
            .NotEmpty()
            .WithMessage("Message content is required")
            .MaximumLength(4000)
            .WithMessage("Message content cannot exceed 4000 characters");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid message type");

        RuleFor(x => x.Content.Attachment)
            .SetValidator(new ChatMessageAttachmentValidator())
            .When(x => x.Content.Attachment is not null);
    }
}

public class ChatMessageAttachmentValidator : AbstractValidator<MessageAttachment>
{
    public ChatMessageAttachmentValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("File name is required");

        RuleFor(x => x.FileUrl)
            .NotEmpty()
            .WithMessage("File URL is required");

        RuleFor(x => x.FileSize)
            .GreaterThan(0)
            .WithMessage("File size must be greater than 0")
            .LessThanOrEqualTo(10485760) // 10MB
            .WithMessage("File size cannot exceed 10MB");
    }
}
