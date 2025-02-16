using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ITKarieraAnketiWeb.Models.ViewModels
{
    public class SurveyCreatorViewModel
    {
        [Required(ErrorMessage = "Survey title is required")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; }

        [EnsureMinimumOneQuestion(ErrorMessage = "At least one question is required")]
        public List<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
    }

    public class QuestionViewModel
    {
        [Required(ErrorMessage = "Question text is required")]
        [StringLength(500, ErrorMessage = "Question cannot exceed 500 characters")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Question type is required")]
        public string QuestionType { get; set; } = "MultipleChoice";

        [Required(ErrorMessage = "Order number is required")]
        [Range(1, 100, ErrorMessage = "Invalid order number")]
        public int OrderNumber { get; set; }

        [EnsureOptionsWhenMultipleChoice(ErrorMessage = "Multiple choice questions require at least 2 options")]
        public List<string> Options { get; set; } = new List<string>();
    }

    // Custom validation attributes
    public class EnsureMinimumOneQuestion : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var list = value as List<QuestionViewModel>;
            if (list?.Count < 1) return new ValidationResult(ErrorMessage);
            return ValidationResult.Success;
        }
    }

    public class EnsureOptionsWhenMultipleChoice : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var question = context.ObjectInstance as QuestionViewModel;
            if (question?.QuestionType == "MultipleChoice")
            {
                var options = value as List<string>;
                if (options == null || options.Count(o => !string.IsNullOrWhiteSpace(o)) < 2)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}