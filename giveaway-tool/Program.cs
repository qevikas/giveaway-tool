using giveaway_tool.Contracts;
using giveaway_tool.Library;
using giveaway_tool.Services;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register SignalR
builder.Services.AddSignalR();

// Register Youtube Service and ApiKey
builder.Services.AddSingleton(new YouTubeService(new BaseClientService.Initializer
{
    ApiKey = builder.Configuration["AppSettings:ApiKey"],
    ApplicationName = "YouTube Comment Picker"
}));

// Memory Cache with bounded size limit to prevent excessive memory usage
builder.Services.AddMemoryCache(options => { options.SizeLimit = 10_000; });

// Data Protection for secure token handling
builder.Services.AddDataProtection().SetApplicationName("GiveawayTool");

#region Register Custom Services

builder.Services.AddScoped<IYouTube, YouTube>();
builder.Services.AddScoped<ICommon, Common>();
builder.Services.AddSingleton<IWorkspaceStore, WorkspaceStore>();
builder.Services.AddSingleton<IWorkspaceCookieService, WorkspaceCookieService>();

#endregion

// Antiforgery protection
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = "__Host-GiveawayAntiForgery";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.Path = "/";
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("WorkspacePolicy", limiter =>
    {
        limiter.PermitLimit = 30;
        limiter.Window = TimeSpan.FromMinutes(1);
        limiter.QueueLimit = 0;
        limiter.AutoReplenishment = true;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
