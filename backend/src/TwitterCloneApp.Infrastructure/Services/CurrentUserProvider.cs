using TwitterCloneApp.Application.Common.Interfaces;
using TwitterCloneApp.Infrastructure.Persistence.Seed;

namespace TwitterCloneApp.Infrastructure.Services;

public class CurrentUserProvider : ICurrentUserProvider
{
    public Guid GetCurrentUserId() => SeedConstants.CurrentUserId;
}