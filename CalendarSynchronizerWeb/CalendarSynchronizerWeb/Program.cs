using BLL.Intrfaces;
using BLL.Services;
using CalendarSynchronizerWeb.Models;
using DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.ConfigureWritable<GoogleAuthCreds>(builder.Configuration.GetSection("GoogleAuthCreds"));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession();
//builder.Services.AddScoped<IConfigurationManagerService<GoogleAuthCreds>, ConfigurationManagerService<GoogleAuthCreds>>();
builder.Services.AddScoped<ISha256HelperService, Sha256HelperService>();
builder.Services.AddScoped<IGoogleOAuthService, GoogleOAuthService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
