namespace ITKarieraAnketiWeb.Models
{
    public class Answer
    {
        public Guid AnswerId { get; set; } = Guid.NewGuid();
        public string AnswerText { get; set; }
        public Guid ResponseId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid? OptionId { get; set; } 
        public Response Response { get; set; }
        public Question Question { get; set; }
        public Option Option { get; set; }
    }
}
