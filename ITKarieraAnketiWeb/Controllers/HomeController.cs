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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state");
                return View(model);
            }

            try
            {

                // Check if username exists
                if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View("Register", model);
                }

                // Hash password
                var salt = Hasher.GenerateSalt();
                var passwordHash = Hasher.HashPassword(model.Password, salt);

                var user = new User
                {
                    Username = model.Username,
                    PasswordHash = passwordHash,
                    Salt = salt,
                    Surveys = new List<Survey>(),
                    Responses = new List<Response>()
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Sign in user immediately after registration
                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                };

                // Sign in user with cookie authentication
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed");
                ModelState.AddModelError("", "An error occurred during registration");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state");
                return View(model);
            }
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
                if (user == null || !Hasher.VerifyPassword(model.Password, user.PasswordHash, user.Salt))
                {
                    ModelState.AddModelError("", "Invalid username or password");
                    return View("Login", model);
                }

               // Create claims for user
                var claims = new List<Claim> 
                {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                };

                // Sign in user with cookie authentication
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

                return RedirectToAction("HomePage");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed");
                ModelState.AddModelError("", "An error occurred during login");
                return View(model);
            }
        }

        [Authorize]
        public IActionResult HomePage()
        {
            return View(new HomePageViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Logout(HomePageViewModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            await HttpContext.SignOutAsync();
            return RedirectToAction("/Home/Index");
        }
    }
}

