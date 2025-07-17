namespace Clickly.Data.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Text { get; set; } = string.Empty;
        public string? UploadedFileUrl { get; set; } 
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}
