using Azure;

namespace ITKarieraAnketiWeb.Models
{
    public class User
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Survey> Surveys { get; set; }
        public List<Response> Responses { get; set; }
    }
}
