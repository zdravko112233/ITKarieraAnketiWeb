using System.ComponentModel.DataAnnotations;
namespace ITKarieraAnketiWeb.Models.ViewModels
{
    public class HomePageViewModel
    {
        public string Username { get; set; }
        public List<Survey> Surveys { get; set; }
    }
}
