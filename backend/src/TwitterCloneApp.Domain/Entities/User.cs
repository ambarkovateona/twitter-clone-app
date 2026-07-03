namespace TwitterCloneApp.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;

    public ICollection<Post> Posts { get; set; } = new List<Post>();
}