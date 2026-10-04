using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nkklession14layout.Data;
using Nkklession14layout.Models;
using Nkklession14layout.Services;

var builder = WebApplication.CreateBuilder(args);

// Keep application logs on the console so startup and request logging do not
// depend on Windows Event Log permissions.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is not configured.");
builder.Services.AddDbContext<NkkApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<PasswordHasher<NkkTaiKhoanModel>>();
builder.Services.AddTransient<NkkIEmailSender, NkkSmtpEmailSender>();
builder.Services
    .AddAuthentication("ShopCookie")
    .AddCookie("ShopCookie", options =>
    {
        options.LoginPath = "/NkkAccount/NkkLogin";
        options.AccessDeniedPath = "/NkkAccount/NkkLogin";
        options.Cookie.Name = "PhoneShop.Auth";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });
builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

