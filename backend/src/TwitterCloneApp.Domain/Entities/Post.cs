namespace TwitterCloneApp.Domain.Entities;

public class Post
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid AuthorId { get; set; }
    public User Author { get; set; } = null!;
    public string? ImageUrl { get; set; }
}