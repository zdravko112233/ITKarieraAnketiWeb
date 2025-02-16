using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKarieraAnketiWeb.Models
{
    public class Option
    {
        public Guid OptionId { get; set; }
        public string OptionText { get; set; }

        [ForeignKey("Question")]
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        public int OrderNumber { get; set; } 
    }
}