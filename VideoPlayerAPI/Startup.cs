using MediatR;
using Microsoft.EntityFrameworkCore;
using VideoPlayer.Web.Seeder;
using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.BusinessLogic.Infrastructure.Extensions;

namespace VideoPlayerAPI;

public class Startup(IConfiguration configuration, IWebHostEnvironment environment)
{
    public IConfiguration Configuration { get; } = configuration;
    public IWebHostEnvironment Environment { get; } = environment;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddApplicationServices(Configuration);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddCQRSServices();

        services.AddIdentityServices(Configuration);
    }

    public async void Configure(IApplicationBuilder app)
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

        app.UseCors("CorsPolicy");
        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<VideoPlayerDbContext>();
            var sender = services.GetRequiredService<ISender>();
            await context.Database.MigrateAsync();
            await Seed.SeedUsers(context);
            await Seed.SeedVideos(context, sender);
        }
        catch (Exception ex)
        {
            var logger = services.GetService<ILogger<Startup>>();
            logger.LogError(ex, "An error occured during migration");
        }

    }
}
