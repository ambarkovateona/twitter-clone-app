using MediatR;
using TwitterCloneApp.Application.Posts.Dtos;

namespace TwitterCloneApp.Application.Posts.Commands.CreatePost;

public record CreatePostCommand(string Content, string? ImageUrl) : IRequest<PostDto>;