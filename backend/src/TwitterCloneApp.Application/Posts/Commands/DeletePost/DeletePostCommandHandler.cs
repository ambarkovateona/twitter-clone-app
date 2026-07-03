using MediatR;
using Microsoft.EntityFrameworkCore;
using TwitterCloneApp.Application.Common.Exceptions;
using TwitterCloneApp.Application.Common.Interfaces;

namespace TwitterCloneApp.Application.Posts.Commands.DeletePost;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserProvider _currentUserProvider;

    public DeletePostCommandHandler(IApplicationDbContext dbContext, ICurrentUserProvider currentUserProvider)
    {
        _dbContext = dbContext;
        _currentUserProvider = currentUserProvider;
    }

    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        var post = await _dbContext.Posts
            .FirstOrDefaultAsync(p => p.Id == request.PostId, cancellationToken);

        if (post is null)
{
    throw new NotFoundException($"Post with Id '{request.PostId}' was not found.");
}

if (post.AuthorId != currentUserId)
{
    throw new ForbiddenAccessException("You cannot delete someone else's post.");
}

        _dbContext.Posts.Remove(post);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}