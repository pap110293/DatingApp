namespace DatingApp.API.Helpers
{
    public class MessageParams : PagingParams
    {
        public long UserId { get; set; }
        public string MessageContainer { get; set; } = "Unread";
    }
}