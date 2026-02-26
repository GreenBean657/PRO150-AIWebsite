using PRO150_Website.Components.Models;
namespace PRO150_Website.Components.Models
{
    public class MessageBuilder
    {
        public ChatMessage BuildMsg(List<string> activeChat)
        {
            var builtStr = "";
            foreach (var chat in activeChat)
            {
                builtStr += (chat + "\n");
            }
            return new ChatMessage{role = "user", content = builtStr};
        }
    }

    public class SystemPromptBuilder
    {
        public ChatMessage BuildMsg(string prompt)
        {
            return new ChatMessage{role = "system", content = prompt};
        }
    }
}