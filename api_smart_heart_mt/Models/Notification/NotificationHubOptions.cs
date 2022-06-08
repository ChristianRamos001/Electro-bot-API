using System.ComponentModel.DataAnnotations;

namespace API.Smart_Heart.Models.Notification
{
    public class NotificationHubOptions
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ConnectionString { get; set; }
    }
}
