using Microsoft.AspNetCore.Mvc;
using ITKarieraAnketiWeb.Models.ViewModels;

namespace ITKarieraAnketiWeb.Controllers
{
    public class SurveyController : Controller
    {
        public IActionResult SurveyCreator()
        {
            return View(new SurveyCreatorViewModel());
        }
    }
}
