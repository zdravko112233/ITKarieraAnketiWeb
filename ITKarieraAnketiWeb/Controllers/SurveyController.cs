using System.Diagnostics;
using ITKarieraAnketiWeb.Models;
using ITKarieraAnketiWeb.Database;
using ITKarieraAnketiWeb.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using ITKarieraAnketiWeb.Models.ViewModels;

namespace ITKarieraAnketiWeb.Controllers
{
    public class SurveyController : Controller
    {
        private readonly ILogger<SurveyController> _logger;
        private readonly AppDbContext _context;

        public SurveyController(ILogger<SurveyController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult SurveyCreator()
        {
            return View(new SurveyCreatorViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SurveyCreator(SurveyCreatorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var survey = new Survey
                {
                    Title = model.Title,
                    Description = model.Description,
                    Questions = model.Questions
                };
                await _context.Surveys.AddAsync(survey);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
