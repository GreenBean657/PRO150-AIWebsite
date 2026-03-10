namespace PRO150_Website.Components.Models
{
    public class AIModel
    {
        public string Model { get; set; }
    }

    public static class AllAIModels {
        public static List<string> AIModels = new() { "Model1", "Model2", "Model3" };
    }
}