using System.ComponentModel.DataAnnotations;

namespace PRO150_Website.Components.Models
{
    public class Message
    {
        [Required]
        public string message { get; set; }
    }
}
