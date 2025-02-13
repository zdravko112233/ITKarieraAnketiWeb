using ITKarieraAnketiWeb.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("Default"),
        new MySqlServerVersion(new Version(5, 5, 62)),
        mysqlOptions => {
        mysqlOptions.EnableRetryOnFailure();
        mysqlOptions.CommandTimeout(60);
        }
    )
);
builder.Services.AddScoped<UserRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/Home/Login";    
        options.AccessDeniedPath = "/Home/Error";  
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
    });

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 

app.MapGet("/", () => Results.Redirect("/Home/Index"));

app.Run();