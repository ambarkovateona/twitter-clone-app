using MediatR;
using TwitterCloneApp.Application.Common.Models;
using TwitterCloneApp.Application.Posts.Dtos;

namespace TwitterCloneApp.Application.Posts.Queries.GetMyPosts;

public record GetMyPostsQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResult<PostDto>>;