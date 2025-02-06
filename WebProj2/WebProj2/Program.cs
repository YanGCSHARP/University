using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using WebProj2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using NuGet.Configuration;
using WebProj2.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var mainConnetionString = builder.Configuration.GetConnectionString("ApplicationDbConnection");
var authConnectionString = builder.Configuration.GetConnectionString("AuthDbConnection");

builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseNpgsql(authConnectionString);
});
builder.Services.AddDefaultIdentity<Users>(options =>
    {
        // Настройка требований к паролю
        options.Password.RequireDigit = false; // Не требуется цифра
        options.Password.RequireLowercase = false; // Не требуется строчная буква
        options.Password.RequireNonAlphanumeric = false; // Не требуются неалфавитные символы
        options.Password.RequireUppercase = false; // Не требуется прописная буква
        options.Password.RequiredLength = 3; // Минимальная длина пароля
        options.Password.RequiredUniqueChars = 0; // Количество уникальных символов не требуется
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(mainConnetionString);
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Version = "v1",
        Title = " NLP",
        Description = "(:",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        } });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "SuperAdmin", "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();

    string email = "superadmin@test.test";
    string password = "Test123!";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new Users() { UserName = email, Email = email, EmailConfirmed = true};
        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "SuperAdmin");
    }
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSwaggerUI();
app.UseSwagger();
app.UseRouting();

app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();