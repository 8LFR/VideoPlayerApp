using Microsoft.AspNetCore.Mvc;
using VideoPlayerAPI.Models;

namespace VideoPlayerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideosController(IWebHostEnvironment environment, VideoPlayerDbContext dbContext) : ControllerBase
    {
        private static readonly List<Video> videos =
        [
            new Video { 
                Id = 1, 
                Title = "Sample Video 1", 
                Description = "Description of sample video 1", 
                FilePathOrUrl = "https://example.com/video1.mp4",
                FileName = "video1",
                FileSize = 1024,
                ContentType = "video/mp4",
                Duration = TimeSpan.FromMinutes(5),
                UploadDate = DateTime.UtcNow},
            new Video {
                Id = 2,
                Title = "Sample Video 2",
                Description = "Description of sample video 2",
                FilePathOrUrl = "https://example.com/video2.mp4",
                FileName = "video2",
                FileSize = 2048,
                ContentType = "video/mp4",
                Duration = TimeSpan.FromMinutes(10),
                UploadDate = DateTime.UtcNow}
        ];

        private readonly IWebHostEnvironment _environment = environment;
        private readonly VideoPlayerDbContext _dbContext = dbContext;

        // GET: api/videos
        [HttpGet]
        public ActionResult<IEnumerable<Video>> GetVideos()
        {
            return Ok(videos);
        }

        // GET: api/videos/{id}
        [HttpGet("{id}")]
        public ActionResult<Video> GetVideoById(int id)
        {
            var video = videos[id];
            if (video == null)
            {
                return NotFound();
            }

            return Ok(video);
        }

        // POST: api/videos/upload
        [HttpPost("upload")]
        public async Task<ActionResult<Video>> UploadVideo(IFormFile file, [FromBody] Video video)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var filePath = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", filePath);

            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //var relativePath = Path.Combine("uploads", fileName).Replace("\\", "/");
            //var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            //var fileUrl = $"{baseUrl}/{relativePath}";

            video.FilePathOrUrl = filePath;
            video.FileName = file.FileName;
            video.FileSize = (int)Math.Min(file.Length, int.MaxValue);
            video.ContentType = file.ContentType;
            video.UploadDate = DateTime.Now;

            _dbContext.Videos.Add(video);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVideoById), new { video.Id}, video);
        }

        // POST: api/videos
        [HttpPost]
        public ActionResult<Video> CreateVideo([FromBody] Video video)
        {
            video.Id = videos.Count + 1;
            video.UploadDate = DateTime.UtcNow;
            videos.Add(video);

            return CreatedAtAction(nameof(GetVideoById), new { video.Id }, video);
        }

        // PUT: api/videos/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateVideo(int id, [FromBody] Video updatedVideo)
        {
            var videoIndex = videos.FindIndex(v => v.Id == id);
            if (videoIndex == -1)
            {
                return NotFound();
            }

            updatedVideo.Id = id;
            videos[videoIndex] = updatedVideo;

            return NoContent();
        }

        // DELETE: api/videos/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteVideo(int id)
        {
            var videoToRemove = videos[id];
            if (videoToRemove == null)
            {
                return NotFound();
            }

            videos.Remove(videoToRemove);

            return NoContent();
        }
    }
}
