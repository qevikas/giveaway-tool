using giveaway_tool.Library;
using giveaway_tool.Services;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register SignalR
builder.Services.AddSignalR();

// Register Services
builder.Services.AddScoped<IYouTube, YouTube>();
builder.Services.AddScoped<ICommon, Common>();

// Register Youtube Service and ApiKey
builder.Services.AddSingleton(new YouTubeService(new BaseClientService.Initializer
{
    ApiKey = builder.Configuration["AppSettings:ApiKey"],
    ApplicationName = "YouTube Comment Picker"
}));

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
