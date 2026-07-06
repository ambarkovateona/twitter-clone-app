using MediatR;
using Microsoft.EntityFrameworkCore;
using TwitterCloneApp.Application.Common.Interfaces;
using TwitterCloneApp.Application.Common.Models;
using TwitterCloneApp.Application.Posts.Dtos;

namespace TwitterCloneApp.Application.Posts.Queries.GetAllPosts;

public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, PagedResult<PostDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllPostsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<PostDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Posts
            .Include(p => p.Author)
            .OrderByDescending(p => p.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new PostDto
            {
                Id = p.Id,
                AuthorUsername = p.Author.Username,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                ImageUrl = p.ImageUrl
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<PostDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}