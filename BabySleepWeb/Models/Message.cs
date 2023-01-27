using BabySleep.Resources.Resx;
using System.ComponentModel.DataAnnotations;

namespace BabySleepWeb.Models
{
    public class Message
    {
        [Required(ErrorMessageResourceName = "EmptyEmail", ErrorMessageResourceType = typeof(ContactResources))]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(ContactResources))]
        public string From { get; set; }
        [Required(ErrorMessageResourceName = "EmptySubject", ErrorMessageResourceType = typeof(ContactResources))]
        public string Subject { get; set; }
        [Required(ErrorMessageResourceName = "EmptyBody", ErrorMessageResourceType = typeof(ContactResources))]
        public string Body { get; set; }
    }
}
