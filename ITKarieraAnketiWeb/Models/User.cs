using Azure;

namespace ITKarieraAnketiWeb.Models
{
    public class User
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string Email { get; set; }
        public byte PasswordHash { get; set; }
        public byte Salt { get; set; }   
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<Survey> Surveys { get; set; }
        public List<Response> Responses { get; set; }
    }
}
