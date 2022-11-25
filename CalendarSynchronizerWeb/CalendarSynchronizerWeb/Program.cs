using BLL.Intrfaces;
using BLL.Services;
using CalendarSynchronizerWeb.Authorization;
using CalendarSynchronizerWeb.Extensions;
using CalendarSynchronizerWeb.Helpers;
using CalendarSynchronizerWeb.Models;
using CalendarSynchronizerWeb.Services;
using CalendarSynchronizerWeb.Services.Interfaces;
using Core.Models;
using DAL;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddLogging();
builder.Services.ConfigureWritable<GoogleAuthCreds>(builder.Configuration.GetSection("GoogleAuthCreds"));
builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            x => x.MigrationsAssembly("CalendarSynchronizerWeb")));

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration.GetSection("SendGrid"));
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = "516669132332-ril060aftkfjnc8gq5mqurffc3t95n8q.apps.googleusercontent.com";
        options.ClientSecret = "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";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OnlyAdminChecker", policy => policy.Requirements.Add(new OnlyAdminAuthorization()));
    options.AddPolicy("CheckUserNameTeddy", policy => policy.Requirements.Add(new UserNameRequirement("teddy")));
});


builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequiredLength = 4;
    opt.Password.RequireLowercase = true;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(20);
    opt.Lockout.MaxFailedAccessAttempts = 5;
    
});

builder.Services.AddSession();

builder.Services.AddScoped<ICalendarRepository, CalendarRepository>();


builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<ISha256HelperService, Sha256HelperService>();
builder.Services.AddScoped<IGoogleOAuthService, GoogleOAuthService>();
builder.Services.AddTransient<ISendGridEmailService, SendGridEmailService>();

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

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
