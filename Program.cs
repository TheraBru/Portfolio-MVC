using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using netProject.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using netProject.Services;
using System.Net;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//Add connection to Identity Framework and SQLite database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Add login confirmation requirements
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//Add connection to Entity Framework and SQLite database
builder.Services.AddDbContext<PortfolioDbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("WebsiteDbString")));

//Add connection to Email elements
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

builder.Services.AddHttpClient();

//For publishing to Heroku
//builder.WebHost.ConfigureKestrel((context, serverOptions) =>
//{
//    var kestrelSection = context.Configuration.GetSection("Kestrel");

//    serverOptions.Configure(kestrelSection)
//        .Endpoint("HTTPS", listenOptions =>
//        {

//        });
//});

//builder.WebHost.UseKestrel(options =>
//    {
//        options.Listen(IPAddress.Any, Int32.Parse(Environment.GetEnvironmentVariable("PORT")));
//    });

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

