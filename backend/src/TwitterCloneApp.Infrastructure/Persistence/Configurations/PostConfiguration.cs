using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TwitterCloneApp.Domain.Entities;

namespace TwitterCloneApp.Infrastructure.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Content)
            .IsRequired()
            .HasMaxLength(140);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(300);
    }
}