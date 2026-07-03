namespace TwitterCloneApp.Application.Posts.Dtos;

public class PostDto
{
    public Guid Id { get; set; }
    public string AuthorUsername { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? ImageUrl { get; set; }
}