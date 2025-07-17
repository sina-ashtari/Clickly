namespace Clickly.ViewModels.Chats
{
    public class ChatPreviewVM
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerProfilePicture { get; set; }
        public string LastMessage { get; set; }
        public DateTime SentAt { get; set; }
    }
}
