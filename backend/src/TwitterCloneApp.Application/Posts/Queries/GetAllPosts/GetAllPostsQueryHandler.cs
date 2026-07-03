using MediatR;
using Microsoft.EntityFrameworkCore;
using TwitterCloneApp.Application.Common.Interfaces;
using TwitterCloneApp.Application.Posts.Dtos;

namespace TwitterCloneApp.Application.Posts.Queries.GetAllPosts;

public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, List<PostDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllPostsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<PostDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Posts
            .Include(p => p.Author)
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