using Devspot.Data;
using Devspot.Models;
using Devspot.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("database"));
});

builder.Services.AddScoped<IRepository<JobPosting>, JobPostingRepository>();

builder.Services.AddDefaultIdentity<IdentityUser>(opt =>
{
    opt.SignIn.RequireConfirmedAccount = false; // Development
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope()) // Accessing Scoped Services
{
    var services = scope.ServiceProvider; // Gets all Services
    RoleSeeder.SeedRolesAsync(services).Wait();
    UserSeeder.SeedUsersAsync(services).Wait();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();


app.MapStaticAssets();
app.MapRazorPages();
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();