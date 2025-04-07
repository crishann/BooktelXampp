using Microsoft.EntityFrameworkCore;
using NewBooktel.Data;
using NewBooktel.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Email Auth
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

// ✅ Fix Database Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 23)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure()
    ));

// ✅ Register RoomService
builder.Services.AddScoped<RoomService>();

// ✅ Enable Sessions (for login authentication)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // ⏳ User stays logged in for 30 mins
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// ✅ Fix CSRF Cookie Issues
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // ✅ Auto-detect HTTP/HTTPS
    options.Cookie.SameSite = SameSiteMode.Lax; // ✅ Prevent cross-site issues
});

// ✅ Add MVC Controllers & Views
builder.Services.AddControllersWithViews();

// ✅ Add Authentication Services (Cookie Authentication)
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth/Login"; // Redirect here when unauthenticated
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Optional: Redirect for access denied
    });

var app = builder.Build(); // 🚨 NO service registrations after this line!

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ✅ Use Authentication Middleware
app.UseAuthentication(); // Add this line

// ✅ Enable Sessions before Authorization
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"); // Default route to Login


app.Run();
