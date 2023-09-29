using Airline_Resevation_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Airline_Resevation_System.Seeder;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("AirlineDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AirlineDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();
var provider = builder.Services.BuildServiceProvider();
var config = provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContext<AirlineDbContext>(item => item.UseSqlServer(config.GetConnectionString("dbcs")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AirlineDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// add scope for seeder data 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager=services.GetRequiredService<UserManager<IdentityUser>>();
    SeedData.SeedAdminUserAsync(userManager).Wait();
}
app.MapRazorPages();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
