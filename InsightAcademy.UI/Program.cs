
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InsightAcademy.InfrastructureLayer.Data;
using DNTCaptcha.Core;
using InsightAcademy.InfrastructureLayer.Data.Seed;
using InsightAcademy.DomainLayer.Entities.Identity;
using InsightAcademy.ApplicationLayer.Services;
using InsightAcademy.InfrastructureLayer.Implementation;
using InsightAcademy.ApplicationLayer.Repository;
using InsightAcademy.UI.Helper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;



// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddRazorPages();
builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<ITutorService, TutorServices>();
builder.Services.AddScoped<ITutor, TutorRepository>();
builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddScoped<IWishList, WishListRepository>();
builder.Services.AddScoped<IEmailSender, EmailService>();
builder.Services.AddScoped<IApplicationEmailSender, EmailService>();
builder.Services.AddScoped<FileUploader>();
builder.Services.AddSingleton<IHostedService, DefaultUserAndRoles>();

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];

});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";

});
builder.Services.AddDNTCaptcha(options =>
{
    options.UseCookieStorageProvider().ShowThousandsSeparators(false);
    options.WithEncryptionKey("nsdjvnsdjwr454wr35");

});

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
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );
});
app.MapRazorPages();

app.Run();
