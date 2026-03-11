using System.Text.Json.Serialization;

namespace PRO150_Website.Components.Models
{
    public class AIModel
    {
        public string Model { get; set; }
    }

    // Matches API response format
    public class AIModelApi
    {
        public string id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        public string owned_by { get; set; }
    }

    public class AIModelListApi
    {
        public AIModelApi[] data { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }
    }

    public static class AllAIModels
    {
        public static List<AIModel> AIModels { get; set; } = new();
    }
}