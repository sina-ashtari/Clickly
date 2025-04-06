using System.ComponentModel.DataAnnotations;

namespace Clickly.Data.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public required string Content { get; set; }
        // in moment that maybe you will have more than 2 situation like friend only, private and public post, then create an enum class and change it to it.
        public bool IsPrivate { get; set; }
        public string? Image { get; set; }
        public int NumberOfReport { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; }

        // foregin key 
        public int UserId { get; set; }
        // Navigation Properties 
        public User User { get; set; }
        public ICollection<Like> Like { get; set; } = new List<Like>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
