using FluentValidation;

namespace TwitterCloneApp.Application.Posts.Commands.CreatePost;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Content)
            .Must(content => !string.IsNullOrWhiteSpace(content))
                .WithMessage("Content cannot be empty.")
            .Must(content => content.Trim().Length >= 12)
                .WithMessage("Content must be at least 12 characters long.")
            .Must(content => content.Trim().Length <= 140)
                .WithMessage("Content must not exceed 140 characters.");
    }
}