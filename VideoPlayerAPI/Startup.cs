using Microsoft.EntityFrameworkCore;

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
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
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
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
