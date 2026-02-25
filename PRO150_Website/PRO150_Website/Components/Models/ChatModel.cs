namespace PRO150_Website.Components.Models
{
    public class ChatMessage
    {

        public string role { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
    }

    public class ChatRequest
    {
        public string model { get; set; } = string.Empty;
        public List<ChatMessage> messages { get; set; } = new();
        public bool stream { get; set; } = true;
    }    
}
