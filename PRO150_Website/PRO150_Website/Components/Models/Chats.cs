namespace PRO150_Website.Components.Models
{
    // Temporary to show how the history page could work
    public class Chats
    {
        public static List<List<string>> allChats = new List<List<string>>();

        public static List<string> activeChat = new List<string>();

        static Chats()
        {
            allChats.Add(activeChat);
        }

        public static void NewChat()
        {
            activeChat = new List<string>();
            allChats.Add(activeChat);
        }

        public static void SetActive(List<string> newActiveChat)
        {
            activeChat = newActiveChat;
        }
    }
}
