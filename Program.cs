using LibraryBook.Areas.Data;
using LibraryBook.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.UI.Services;
using LibraryBook.Services;
using NETCore.MailKit.Infrastructure.Internal;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using LibraryBook.Middlewares;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configure services for the application
builder.Services.AddDbContext<LibraryBook.Areas.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryBookContext") ?? throw new InvalidOperationException("Connection string 'LibraryBookContext' not found.")));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<LibraryBook.Areas.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<LibraryUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<LibraryBook.Areas.Data.ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();

// Add services for RESTful API
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LibraryBook", Version = "v1" });
});

// Add authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CreateLoanPolicy", policy =>
        policy.RequireRole("User", "Admin"));
});

// Add localization support
builder.Services.AddLocalization(options => options.ResourcesPath = "Translations");
builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

var app = builder.Build();

// Configure supported cultures for localization
var supportedCultures = new[] { "en", "fr", "nl" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

// Custom middleware to manage user table
app.UseUserTableMiddleware();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Use the custom error handling middleware without registering it as a service
app.UseMiddleware<CustomErrorHandlingMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<LibraryUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await SeedDatacontext.Initialize(services, userManager, roleManager);
}

// Set up endpoints for controllers
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

// Start the application
app.Run();