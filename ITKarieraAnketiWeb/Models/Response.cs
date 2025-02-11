namespace ITKarieraAnketiWeb.Models
{
    public class Response
    {
        public Guid ResponseId { get; set; } = Guid.NewGuid();
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public Guid SurveyId { get; set; }
        public Guid? UserId { get; set; } 
        public Survey Survey { get; set; }
        public User User { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
