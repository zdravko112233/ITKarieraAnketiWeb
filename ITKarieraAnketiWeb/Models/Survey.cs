using Azure;

namespace ITKarieraAnketiWeb.Models
{
    public class Survey
    {
        public Guid SurveyId { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public List<Question> Questions { get; set; }
        public List<Response> Responses { get; set; }
    }
}