using LibraryBook.Areas.Data;
using LibraryBook.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.UI.Services;
using LibraryBook.Services;
using NETCore.MailKit.Infrastructure.Internal;
using LibraryBook.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<LibraryBook.Areas.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryBookContext") ?? throw new InvalidOperationException("Connection string 'LibraryBookContext' not found.")));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<LibraryBook.Areas.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<LibraryUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<LibraryBook.Areas.Data.ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();

///Email confirmation
/**builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();
builder.Services.Configure<MailKitOptions>(options =>
{
    options.Server = builder.Configuration["ExternalProviders:MailKit:SMTP:Address"];
    options.Port = Convert.ToInt32(builder.Configuration["ExternalProviders:MailKit:SMTP:Port"]);
    options.Account = builder.Configuration["ExternalProviders:MailKit:SMTP:Account"];
    options.Password = builder.Configuration["ExternalProviders:MailKit:SMTP:Password"];
    options.SenderEmail = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderEmail"];
    options.SenderName = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderName"];
    // Set it to TRUE to enable ssl or tls, FALSE otherwise
    options.Security = false;
}); **/



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<LibraryUser>>();
    await SeedDatacontext.Initialize(services, userManager);
}

app.Run();
