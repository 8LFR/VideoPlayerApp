using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using VideoPlayer.Web.Data;
using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.Abstractions.Models;
using VideoPlayerAPI.BusinessLogic.Videos.Queries;
using VideoPlayerAPI.Infrastructure.Video;

namespace VideoPlayer.Web.Seeder;

public class Seed
{
    public static async Task SeedUsers(VideoPlayerDbContext dbContext)
    {
        if (await dbContext.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var users = JsonSerializer.Deserialize<List<User>>(userData);

        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();

            user.Name = user.Name.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            user.PasswordSalt = hmac.Key;

            dbContext.Users.Add(user);
        }

        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedVideos(VideoPlayerDbContext dbContext, ISender sender)
    {
        if (await dbContext.Videos.AnyAsync()) return;

        var videoSeedData = await File.ReadAllTextAsync("Data/VideoSeedData.json");

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var models = JsonSerializer.Deserialize<List<VideoSeedDataModel>>(videoSeedData);

        foreach (var model in models)
        {
            using var httpClient = new HttpClient();

            var videoBytes = await httpClient.GetByteArrayAsync(model.Source);

            var videoData = new VideoData
            {
                VideoType = "mp4",
                Bytes = videoBytes
            };

            var command = new UploadVideoCommand
            {
                Title = model.Title,
                Description = model.Description,
                VideoData = videoData,
                RequestedById = model.RequestedById
            };

            await sender.Send(command, CancellationToken.None);
        }

        await dbContext.SaveChangesAsync();
    }
}
