using Microsoft.EntityFrameworkCore;
using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.Infrastructure.AzureStorage;
using VideoPlayerAPI.Infrastructure.Image.Services;
using VideoPlayerAPI.Infrastructure.Image.Storages;
using VideoPlayerAPI.Infrastructure.Video.Helpers;
using VideoPlayerAPI.Infrastructure.Video.Storages;

namespace VideoPlayerAPI;

public class Startup(IConfiguration configuration, IWebHostEnvironment environment)
{
    public IConfiguration Configuration { get; } = configuration;
    public IWebHostEnvironment Environment { get; } = environment;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<VideoPlayerDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("VideoPlayerConnectionString")));

        services.AddControllers();

        //services.AddCors(o => o.AddDefaultPolicy(builder =>
        //{
        //    builder.AllowAnyOrigin()
        //           .AllowAnyMethod()
        //           .AllowAnyHeader();
        //}));

        services.AddCors(options =>
        {
            options.AddPolicy(name: "CorsPolicy",
                builder => builder.WithOrigins("https://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });

        services.AddSingleton<IImageService, ImageService>();
        services.AddSingleton<IVideoStorage, VideoStorage>();
        services.AddSingleton<IAzureStorageHelper, AzureStorageHelper>();
        services.AddSingleton<IVideoHelper, VideoHelper>();
        services.AddSingleton<IImageStorage, ImageStorage>();
    }

    public void Configure(IApplicationBuilder app)
    {
        if (Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (string.IsNullOrWhiteSpace(Environment.WebRootPath))
        {
            Environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseRouting();

        //app.UseCors();
        app.UseCors("CorsPolicy");
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
