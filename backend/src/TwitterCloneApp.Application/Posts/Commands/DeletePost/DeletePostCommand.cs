using MediatR;

namespace TwitterCloneApp.Application.Posts.Commands.DeletePost;

public record DeletePostCommand(Guid PostId) : IRequest;