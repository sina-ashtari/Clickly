using System.ComponentModel.DataAnnotations;

namespace Clickly.Data.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public required string Content { get; set; }
        public string? Image { get; set; }
        public int NumberOfReport { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        // foregin key 
        public int UserId { get; set; }
        // Navigation Properties 
        public User User { get; set; }
        public ICollection<Like> Like { get; set; } = new List<Like>();
    }
}
