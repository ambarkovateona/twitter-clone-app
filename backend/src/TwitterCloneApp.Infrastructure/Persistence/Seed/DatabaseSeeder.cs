using Microsoft.EntityFrameworkCore;
using TwitterCloneApp.Domain.Entities;

namespace TwitterCloneApp.Infrastructure.Persistence.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext)
    {
        if (await dbContext.Users.AnyAsync())
        {
            return;
        }

        var teona = new User { Id = SeedConstants.CurrentUserId, Username = SeedConstants.CurrentUsername };
        var marko = new User { Id = SeedConstants.MarkoUserId, Username = "marko" };
        var ana = new User { Id = SeedConstants.AnaUserId, Username = "ana" };
        var petar = new User { Id = SeedConstants.PetarUserId, Username = "petar" };
        var elena = new User { Id = SeedConstants.ElenaUserId, Username = "elena" };
        var dragan = new User { Id = SeedConstants.DraganUserId, Username = "dragan" };

        await dbContext.Users.AddRangeAsync(teona, marko, ana, petar, elena, dragan);

        var now = DateTime.UtcNow;

        var posts = new List<Post>
        {
            new Post
            {
                Id = Guid.NewGuid(),
                Content = "Just fixed a tricky EF Core bug — turns out it was a missing Include() all along.",
                CreatedAt = now.AddHours(-1),
                AuthorId = teona.Id
            },
            new Post
            {
                Id = Guid.NewGuid(),
                Content = "Weekend hike photos incoming!",
                CreatedAt = now.AddHours(-3),
                AuthorId = dragan.Id,
                ImageUrl = "https://picsum.photos/seed/hike/600/400"
            },
            new Post
            {
                Id = Guid.NewGuid(),
                Content = "Coffee first, code second. Always.",
                CreatedAt = now.AddHours(-5),
                AuthorId = marko.Id
            },
            new Post
            {
                Id = Guid.NewGuid(),
                Content = "Sunset walks are the best way to debug your brain after work.",
                CreatedAt = now.AddHours(-7),
                AuthorId = ana.Id,
                ImageUrl = "https://picsum.photos/seed/sunset/600/400"
            },
            new Post
            {
                Id = Guid.NewGuid(),
                Content = "Anyone else think semicolons in JS are underrated drama?",
                CreatedAt = now.AddHours(-9),
                AuthorId = petar.Id
            },
            new Post
            {
                Id = Guid.NewGuid(),
                Content = "Working on a CQRS project with .NET and Angular, really enjoying it.",
                CreatedAt = now.AddHours(-11),
                AuthorId = teona.Id
            },
            new Post
            {
                Id = Guid.NewGuid(),
                Content = "Redesigned my portfolio site this weekend. Minimal is the way.",
                CreatedAt = now.AddDays(-1).AddHours(-2),
                AuthorId = elena.Id,
                ImageUrl = "https://picsum.photos/seed/desksetup/600/400"
            },
            new Post
            {
                Id = Guid.NewGuid(),
                Content = "Today's a great day for writing clean code and refactoring.",
                CreatedAt = now.AddDays(-1).AddHours(-6),
                AuthorId = marko.Id
            },
            new Post
            {
                Id = Guid.NewGuid(),
                Content = "Deployed my first Kubernetes cluster today — feeling unstoppable.",
                CreatedAt = now.AddDays(-2),
                AuthorId = petar.Id
            },
            new Post
            {
                Id = Guid.NewGuid(),
                Content = "Trying out a new keyboard layout. My fingers are confused.",
                CreatedAt = now.AddDays(-2).AddHours(-8),
                AuthorId = dragan.Id
            },
            new Post
            {
                Id = Guid.NewGuid(),
                Content = "Reading a great book on distributed systems, highly recommend.",
                CreatedAt = now.AddDays(-3),
                AuthorId = elena.Id
            },
            new Post
            {
                Id = Guid.NewGuid(),
                Content = "This is my post to help test the feed with multiple authors.",
                CreatedAt = now.AddDays(-4),
                AuthorId = ana.Id
            },
            new Post
            {
                Id = Guid.NewGuid(),
                Content = "Welcome to my first post on this app!",
                CreatedAt = now.AddDays(-5),
                AuthorId = teona.Id
            }
        };

        await dbContext.Posts.AddRangeAsync(posts);
        await dbContext.SaveChangesAsync(CancellationToken.None);
    }
}