namespace TwitterCloneApp.Application.Common.Interfaces;

public interface ICurrentUserProvider
{
    Guid GetCurrentUserId();
}