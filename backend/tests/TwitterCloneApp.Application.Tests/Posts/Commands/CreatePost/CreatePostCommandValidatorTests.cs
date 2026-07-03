using FluentValidation.TestHelper;
using TwitterCloneApp.Application.Posts.Commands.CreatePost;
using Xunit;

namespace TwitterCloneApp.Application.Tests.Posts.Commands.CreatePost;

public class CreatePostCommandValidatorTests
{
    private readonly CreatePostCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Content_Is_TooShort()
    {
        var result = _validator.TestValidate(new CreatePostCommand("short", null));
        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public void Should_Have_Error_When_Content_Is_TooLong()
    {
        var tooLong = new string('a', 141);
        var result = _validator.TestValidate(new CreatePostCommand(tooLong, null));
        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public void Should_Have_Error_When_Content_Is_OnlyWhitespace()
    {
        var result = _validator.TestValidate(new CreatePostCommand("            ", null));
        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Content_Is_Valid()
    {
        var result = _validator.TestValidate(new CreatePostCommand("This is valid content for a post.", null));
        result.ShouldNotHaveValidationErrorFor(x => x.Content);
    }
}