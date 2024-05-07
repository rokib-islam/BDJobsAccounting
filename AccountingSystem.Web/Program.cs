using AccountingSystem.AppLicationDbContext.AccountingDatabase;
using AccountingSystem.Configurations.Extentions;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContextPool<AccountingDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    options =>
    {
        options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    });
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(6);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    option =>
    {
        option.LoginPath = "/Home/Index";
        option.ExpireTimeSpan = TimeSpan.FromHours(6);
    });

// **Register HttpClient service**
builder.Services.AddHttpClient();  

builder.Services.ConfigureServices(); 

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddHangfire(configuration =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170) 
        .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
        {
            CommandTimeout = TimeSpan.FromMinutes(10) 
        });
});

builder.Services.AddHangfireServer();

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

app.UseSession();

app.UseAuthorization();

app.UseStaticFiles();

app.UseCors("AllowAll");

app.UseHangfireDashboard();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();