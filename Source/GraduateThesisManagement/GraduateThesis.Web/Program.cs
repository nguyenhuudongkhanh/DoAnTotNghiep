using GraduateThesis.ApplicationCore.Authorization;
using GraduateThesis.ApplicationCore.Context;
using GraduateThesis.ApplicationCore.Email;
using GraduateThesis.ApplicationCore.File;
using GraduateThesis.Repository.BLL.Implements;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Web;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;

#nullable disable

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration.GetConnectionString("DbConnection");

builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContextPool<HufiGraduateThesisContext>(options =>
    options.UseSqlServer(connectionString));

IConfigurationSection smtpSection = builder.Configuration.GetSection("SMTP");

builder.Services.AddSingleton<SmtpConfiguration>(new SmtpConfiguration
{
    Host = smtpSection.GetValue<string>("Host"),
    Port = smtpSection.GetValue<int>("Port"),
    User = smtpSection.GetValue<string>("User"),
    Password = smtpSection.GetValue<string>("Password"),
    EnableSsl = smtpSection.GetValue<bool>("EnableSsl"),
    Address = smtpSection.GetValue<string>("Address"),
    DisplayName = smtpSection.GetValue<string>("DisplayName"),
    Enable = smtpSection.GetValue<bool>("Enable")
});

builder.Services.AddScoped(typeof(IEmailService), typeof(SmtpService));
builder.Services.AddScoped(typeof(IAuthorizationManager), typeof(AuthorizationManager));
builder.Services.AddScoped(typeof(IFileManager), typeof(FileManager));
builder.Services.AddScoped(typeof(IRepository), typeof(Repository));

builder.Services.AddMvc().ConfigureApplicationPartManager(apm =>
{
    ApplicationPart applicationPart = apm.ApplicationParts
        .FirstOrDefault(part => part.Name == "DependentLibrary");

    if (applicationPart != null)
    {
        apm.ApplicationParts.Remove(applicationPart);
    }
});

builder.Services.AddSession();

AppConfiguration.ConfigConnectionString(connectionString);
AppConfiguration.ConfigDefaultMessage();
AppConfiguration.ConfigBackupAndRestore(builder.Configuration.GetSection("Backup"));
AppConfiguration.ConfigErrorHandler(builder.Configuration.GetSection("ErrorHandler"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
