using MediatR;
using TwitterCloneApp.Application.Posts.Dtos;

namespace TwitterCloneApp.Application.Posts.Queries.GetMyPosts;

public record GetMyPostsQuery : IRequest<List<PostDto>>;