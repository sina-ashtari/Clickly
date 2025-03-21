namespace Clickly.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public string? ProfilePictureUrl { get; set; }

        // Navigation Properties for one-to-many relationships
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
