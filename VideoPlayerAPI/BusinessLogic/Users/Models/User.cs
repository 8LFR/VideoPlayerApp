namespace VideoPlayerAPI.BusinessLogic.Users.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastActive { get; set; }
        public string AvatarUrl { get; set; }
    }
}
