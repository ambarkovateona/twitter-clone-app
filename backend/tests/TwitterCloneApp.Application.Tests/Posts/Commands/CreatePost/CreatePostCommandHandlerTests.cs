using Microsoft.EntityFrameworkCore;
using Moq;
using TwitterCloneApp.Application.Common.Interfaces;
using TwitterCloneApp.Application.Posts.Commands.CreatePost;
using TwitterCloneApp.Application.Tests.Common;
using TwitterCloneApp.Domain.Entities;
using Xunit;

namespace TwitterCloneApp.Application.Tests.Posts.Commands.CreatePost;

public class CreatePostCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_CreatesPostWithTrimmedContentAndCorrectAuthor()
    {
        // Arrange
        await using var dbContext = TestDbContextFactory.Create();

        var user = new User { Id = Guid.NewGuid(), Username = "teona" };
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var currentUserProviderMock = new Mock<ICurrentUserProvider>();
        currentUserProviderMock.Setup(p => p.GetCurrentUserId()).Returns(user.Id);

        var handler = new CreatePostCommandHandler(dbContext, currentUserProviderMock.Object);
       var command = new CreatePostCommand("   Валидна содржина со празни места околу.   ", null);
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Валидна содржина со празни места околу.", result.Content);
        Assert.Equal("teona", result.AuthorUsername);

        var savedPost = await dbContext.Posts.FirstOrDefaultAsync(p => p.Id == result.Id);
        Assert.NotNull(savedPost);
        Assert.Equal(user.Id, savedPost!.AuthorId);
    }
}