namespace Clickly.Data.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        // Navigations 
        public Post Post { get; set; }
        public User User { get; set; }

    }
}
