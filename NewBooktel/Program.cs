using Microsoft.EntityFrameworkCore;
using NewBooktel.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ Fix Database Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 23))
    ));

// ✅ Fix CSRF Cookie Issues (Move Above `builder.Build()`)
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // ✅ Allow cookies over HTTP (localhost)
    options.Cookie.SameSite = SameSiteMode.Lax; // ✅ Prevent cross-site issues
});

// ✅ Add MVC Controllers & Views
builder.Services.AddControllersWithViews();

var app = builder.Build(); // ⚠️ DO NOT ADD SERVICES AFTER THIS LINE!

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
