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

        app.UseCors("CorsPolicy");
        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
