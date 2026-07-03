using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TwitterCloneApp.Application.Common.Interfaces;
using TwitterCloneApp.Infrastructure.Persistence;
using TwitterCloneApp.Infrastructure.Services;

namespace TwitterCloneApp.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("TwitterCloneAppDb"));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        return services;
    }
}