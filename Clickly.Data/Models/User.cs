namespace Clickly.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool IsDeleted { get; set; }


        // Navigation Properties for one-to-many relationships
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Story> Stories { get; set; } = new List<Story>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();

    }
}
