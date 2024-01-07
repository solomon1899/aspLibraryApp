using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Library.Data;
using Microsoft.AspNetCore.Identity;
using LibraryApp.Data;
using LibraryApp.Areas.Identity.Data;
using DinkToPdf.Contracts;
using DinkToPdf;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<LibraryAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryAppContext") ?? throw new InvalidOperationException("Connection string 'LibraryAppContext' not found.")));

builder.Services.AddDbContext<LibraryAuthContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryAuthContextConnection") ?? throw new InvalidOperationException("Connection string 'LibraryAppContext' not found.")));

builder.Services.AddDefaultIdentity<LibraryAppUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<LibraryAuthContext>();

// Password input validators
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<EmailSender>();

builder.Services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));
builder.Services.AddScoped<PdfService>();

builder.Services.AddRazorPages();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
