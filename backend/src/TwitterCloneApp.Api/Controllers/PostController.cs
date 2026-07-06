using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneApp.Application.Posts.Commands.CreatePost;
using TwitterCloneApp.Application.Posts.Commands.DeletePost;
using TwitterCloneApp.Application.Posts.Dtos;
using TwitterCloneApp.Application.Posts.Queries.GetAllPosts;
using TwitterCloneApp.Application.Posts.Queries.GetMyPosts;
using TwitterCloneApp.Application.Common.Models;

namespace TwitterCloneApp.Api.Controllers;

public class CreatePostRequest
{
    public string Content { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private static readonly string[] AllowedImageContentTypes = { "image/jpeg", "image/png", "image/webp" };
    private const long MaxImageSizeBytes = 5_000_000;

    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PostsController(IMediator mediator, IWebHostEnvironment webHostEnvironment)
    {
        _mediator = mediator;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
public async Task<ActionResult<PagedResult<PostDto>>> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken cancellationToken = default)
{
    var result = await _mediator.Send(new GetAllPostsQuery(page, pageSize), cancellationToken);
    return Ok(result);
}

[HttpGet("mine")]
public async Task<ActionResult<PagedResult<PostDto>>> GetMine(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken cancellationToken = default)
{
    var result = await _mediator.Send(new GetMyPostsQuery(page, pageSize), cancellationToken);
    return Ok(result);
}

    [HttpPost]
    [RequestSizeLimit(6_000_000)]
    public async Task<ActionResult<PostDto>> Create([FromForm] CreatePostRequest request, CancellationToken cancellationToken)
    {
        string? imageUrl = null;

        if (request.Image is not null)
        {
            imageUrl = await SaveImageAsync(request.Image);
        }

        var command = new CreatePostCommand(request.Content, imageUrl);
        var result = await _mediator.Send(command, cancellationToken);
        return Created($"/api/posts/{result.Id}", result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeletePostCommand(id), cancellationToken);
        return NoContent();
    }

    private async Task<string> SaveImageAsync(IFormFile image)
{
    if (!AllowedImageContentTypes.Contains(image.ContentType))
    {
        throw new ValidationException(new[]
        {
            new ValidationFailure(nameof(CreatePostRequest.Image), "Only JPG, PNG and WEBP images are allowed.")
        });
    }

    if (image.Length > MaxImageSizeBytes)
    {
        throw new ValidationException(new[]
        {
            new ValidationFailure(nameof(CreatePostRequest.Image), "The image must not exceed 5MB.")
        });
    }

    var uploadsPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "uploads");
    Directory.CreateDirectory(uploadsPath);

    var extension = Path.GetExtension(image.FileName);
    var fileName = $"{Guid.NewGuid()}{extension}";
    var filePath = Path.Combine(uploadsPath, fileName);

    await using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await image.CopyToAsync(stream);
    }

    return $"/uploads/{fileName}";
}
    }
