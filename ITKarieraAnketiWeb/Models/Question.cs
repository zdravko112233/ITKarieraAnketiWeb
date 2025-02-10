namespace ITKarieraAnketiWeb.Models
{
    public class Question
    {
        public Guid QuestionId { get; set; } = Guid.NewGuid();
        public string QuestionText { get; set; }
        public string QuestionType { get; set; } // "MultipleChoice", "Text", etc.
        public int OrderNumber { get; set; }
        public Guid SurveyId { get; set; }
        public Survey Survey { get; set; }
        public List<Option> Options { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
