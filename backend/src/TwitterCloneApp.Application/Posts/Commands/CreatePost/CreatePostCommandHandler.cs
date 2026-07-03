using MediatR;
using Microsoft.EntityFrameworkCore;
using TwitterCloneApp.Application.Common.Interfaces;
using TwitterCloneApp.Application.Posts.Dtos;
using TwitterCloneApp.Domain.Entities;

namespace TwitterCloneApp.Application.Posts.Commands.CreatePost;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserProvider _currentUserProvider;

    public CreatePostCommandHandler(IApplicationDbContext dbContext, ICurrentUserProvider currentUserProvider)
    {
        _dbContext = dbContext;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        var author = await _dbContext.Users
            .FirstAsync(u => u.Id == currentUserId, cancellationToken);

        var post = new Post
        {
            Id = Guid.NewGuid(),
            Content = request.Content.Trim(),
            CreatedAt = DateTime.UtcNow,
            AuthorId = currentUserId,
            ImageUrl = request.ImageUrl
        };

        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new PostDto
        {
            Id = post.Id,
            AuthorUsername = author.Username,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            ImageUrl = post.ImageUrl
        };
    }
}