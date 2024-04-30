﻿namespace VideoPlayerAPI.BusinessLogic.Users.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}
