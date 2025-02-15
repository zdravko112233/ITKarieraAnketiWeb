using System.Collections.Generic;

namespace ITKarieraAnketiWeb.Models.ViewModels
{
    public class SurveyCreatorViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Question> Questions { get; set; }

    }
}
