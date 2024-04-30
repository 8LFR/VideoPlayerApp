using Microsoft.EntityFrameworkCore;
using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.Infrastructure.Account;
using VideoPlayerAPI.Infrastructure.AzureStorage;
using VideoPlayerAPI.Infrastructure.Image.Services;
using VideoPlayerAPI.Infrastructure.Image.Storages;
using VideoPlayerAPI.Infrastructure.Video.Helpers;
using VideoPlayerAPI.Infrastructure.Video.Storages;

namespace VideoPlayerAPI.BusinessLogic.Infrastructure.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<VideoPlayerDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("VideoPlayerConnectionString")));

        services.AddCors(options =>
        {
            options.AddPolicy(name: "CorsPolicy",
                builder => builder.WithOrigins("https://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        services.AddSingleton<IImageService, ImageService>();
        services.AddSingleton<IVideoStorage, VideoStorage>();
        services.AddSingleton<IAzureStorageHelper, AzureStorageHelper>();
        services.AddSingleton<IVideoHelper, VideoHelper>();
        services.AddSingleton<IImageStorage, ImageStorage>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
