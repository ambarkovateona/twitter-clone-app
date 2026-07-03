using MediatR;
using Microsoft.EntityFrameworkCore;
using TwitterCloneApp.Application.Common.Interfaces;
using TwitterCloneApp.Application.Posts.Dtos;

namespace TwitterCloneApp.Application.Posts.Queries.GetMyPosts;

public class GetMyPostsQueryHandler : IRequestHandler<GetMyPostsQuery, List<PostDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserProvider _currentUserProvider;

    public GetMyPostsQueryHandler(IApplicationDbContext dbContext, ICurrentUserProvider currentUserProvider)
    {
        _dbContext = dbContext;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<List<PostDto>> Handle(GetMyPostsQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        return await _dbContext.Posts
            .Include(p => p.Author)
            .Where(p => p.AuthorId == currentUserId)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PostDto
            {
                Id = p.Id,
                AuthorUsername = p.Author.Username,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                ImageUrl = p.ImageUrl
            })
            .ToListAsync(cancellationToken);
    }
}