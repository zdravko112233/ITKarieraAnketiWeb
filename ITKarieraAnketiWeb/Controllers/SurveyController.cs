using System.Diagnostics;
using ITKarieraAnketiWeb.Models;
using ITKarieraAnketiWeb.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ITKarieraAnketiWeb.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        [Authorize]
        [HttpGet]
        public IActionResult SurveyCreator()
        {
            return View(new SurveyCreatorViewModel
            {
                Questions = new List<QuestionViewModel> {
                    new QuestionViewModel {
                        Options = new List<string> { "", "" },
                        OrderNumber = 1
                    }
                }
            });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SurveyCreator(SurveyCreatorViewModel model)
        {
            try
            {
                ModelState.Clear();
                TryValidateModel(model);

                foreach (var question in model.Questions)
                {
                    if (question.QuestionType == "MultipleChoice" &&
                        question.Options.Count(o => !string.IsNullOrWhiteSpace(o)) < 2)
                    {
                        ModelState.AddModelError("", "Multiple choice questions require at least 2 valid options");
                    }
                }

                if (!ModelState.IsValid)
                {
                    model.Questions ??= new List<QuestionViewModel>();
                    foreach (var q in model.Questions) q.Options ??= new List<string>();
                    return View(model);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!Guid.TryParse(userId, out Guid userGuid))
                    return Unauthorized();

                var survey = new Survey
                {
                    SurveyId = Guid.NewGuid(),
                    Title = model.Title,
                    Description = model.Description,
                    CreatedAt = DateTime.UtcNow,
                    UserId = userGuid
                };

                // Create questions and options
                survey.Questions = model.Questions
                    .Where(q => !string.IsNullOrWhiteSpace(q.QuestionText))
                    .Select((q, index) =>
                    {
                        var question = new Question
                        {
                            QuestionId = Guid.NewGuid(),
                            QuestionText = q.QuestionText,
                            QuestionType = q.QuestionType,
                            OrderNumber = index + 1,
                            SurveyId = survey.SurveyId // Now survey is initialized
                        };

                        if (q.QuestionType == "MultipleChoice")
                        {
                            question.Options = q.Options
                                .Where(o => !string.IsNullOrWhiteSpace(o))
                                .Select((o, oIndex) => new Option
                                {
                                    OptionId = Guid.NewGuid(),
                                    OptionText = o,
                                    OrderNumber = oIndex + 1,
                                    QuestionId = question.QuestionId // Now question is initialized
                                }).ToList();
                        }

                        return question;
                    }).ToList();

                if (!survey.Questions.Any())
                {
                    ModelState.AddModelError("", "At least one valid question is required");
                    return View(model);
                }

                await _context.Surveys.AddAsync(survey);
                await _context.SaveChangesAsync();

                return RedirectToAction("SurveyCreated", new { id = survey.SurveyId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating survey");
                ModelState.AddModelError("", "An error occurred while saving the survey");
                model.Questions ??= new List<QuestionViewModel>();
                return View(model);
            }
        }
        public IActionResult SurveyCreated(Guid id)
        {
            var survey = _context.Surveys
                .Include(s => s.Questions)
                .FirstOrDefault(s => s.SurveyId == id);

            return View(survey);
        }

    }
}