namespace VideoPlayerAPI.Abstractions.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset LastActive { get; set; } = DateTimeOffset.UtcNow;
        public string AvatarFilename { get; set; }
    }
}
