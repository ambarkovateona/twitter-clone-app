using MediatR;
using TwitterCloneApp.Application.Posts.Dtos;

namespace TwitterCloneApp.Application.Posts.Queries.GetAllPosts;

public record GetAllPostsQuery : IRequest<List<PostDto>>;