namespace ITKarieraAnketiWeb.Models
{
    public class Option
    {
        public Guid OptionId { get; set; } = Guid.NewGuid();
        public string OptionText { get; set; }
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
