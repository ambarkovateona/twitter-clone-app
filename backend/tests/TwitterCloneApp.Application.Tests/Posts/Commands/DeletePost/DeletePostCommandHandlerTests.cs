using Moq;
using TwitterCloneApp.Application.Common.Exceptions;
using TwitterCloneApp.Application.Common.Interfaces;
using TwitterCloneApp.Application.Posts.Commands.DeletePost;
using TwitterCloneApp.Application.Tests.Common;
using TwitterCloneApp.Domain.Entities;
using Xunit;

namespace TwitterCloneApp.Application.Tests.Posts.Commands.DeletePost;

public class DeletePostCommandHandlerTests
{
    [Fact]
    public async Task Handle_OwnPost_DeletesSuccessfully()
    {
        await using var dbContext = TestDbContextFactory.Create();

        var user = new User { Id = Guid.NewGuid(), Username = "teona" };
        var post = new Post { Id = Guid.NewGuid(), Content = "Some content here", CreatedAt = DateTime.UtcNow, AuthorId = user.Id };
        dbContext.Users.Add(user);
        dbContext.Posts.Add(post);
        await dbContext.SaveChangesAsync();

        var currentUserProviderMock = new Mock<ICurrentUserProvider>();
        currentUserProviderMock.Setup(p => p.GetCurrentUserId()).Returns(user.Id);

        var handler = new DeletePostCommandHandler(dbContext, currentUserProviderMock.Object);

        await handler.Handle(new DeletePostCommand(post.Id), CancellationToken.None);

        Assert.Empty(dbContext.Posts);
    }

    [Fact]
    public async Task Handle_NonExistentPost_ThrowsNotFoundException()
    {
        await using var dbContext = TestDbContextFactory.Create();

        var currentUserProviderMock = new Mock<ICurrentUserProvider>();
        currentUserProviderMock.Setup(p => p.GetCurrentUserId()).Returns(Guid.NewGuid());

        var handler = new DeletePostCommandHandler(dbContext, currentUserProviderMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new DeletePostCommand(Guid.NewGuid()), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_OtherUsersPost_ThrowsForbiddenAccessException()
    {
        await using var dbContext = TestDbContextFactory.Create();

        var owner = new User { Id = Guid.NewGuid(), Username = "marko" };
        var someoneElse = new User { Id = Guid.NewGuid(), Username = "teona" };
        var post = new Post { Id = Guid.NewGuid(), Content = "Marko's content here", CreatedAt = DateTime.UtcNow, AuthorId = owner.Id };

        dbContext.Users.AddRange(owner, someoneElse);
        dbContext.Posts.Add(post);
        await dbContext.SaveChangesAsync();

        var currentUserProviderMock = new Mock<ICurrentUserProvider>();
        currentUserProviderMock.Setup(p => p.GetCurrentUserId()).Returns(someoneElse.Id);

        var handler = new DeletePostCommandHandler(dbContext, currentUserProviderMock.Object);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            handler.Handle(new DeletePostCommand(post.Id), CancellationToken.None));
    }
}