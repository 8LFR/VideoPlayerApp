using FluentValidation;
using Microsoft.EntityFrameworkCore;
using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.Abstractions.Repositories;
using VideoPlayerAPI.BusinessLogic.Account.Validators;
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
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins("https://localhost:4200")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        services.AddSingleton<IImageService, ImageService>();
        services.AddSingleton<IVideoStorage, VideoStorage>();
        services.AddSingleton<IAzureStorageHelper, AzureStorageHelper>();
        services.AddSingleton<IVideoHelper, VideoHelper>();
        services.AddSingleton<IImageStorage, ImageStorage>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IVideoRepository, VideoRepository>();

        return services;
    }
}
