using MediatR;
using TwitterCloneApp.Application.Posts.Dtos;
using TwitterCloneApp.Application.Common.Models;
namespace TwitterCloneApp.Application.Posts.Queries.GetAllPosts;






public record GetAllPostsQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResult<PostDto>>;