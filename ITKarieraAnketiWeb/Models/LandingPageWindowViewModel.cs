using System.ComponentModel.DataAnnotations;
namespace ITKarieraAnketiWeb.Models
{
    public class LandingPageWindowViewModel
    {
        public string Username { get; set; }
        public List<Survey> Surveys { get; set; }
    }
}
