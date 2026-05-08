using Microsoft.EntityFrameworkCore;
using SporSalonu.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. MVC Servislerini Ekle
builder.Services.AddControllersWithViews();

// 2. Veritabaný Baðlantýsý (SporContext)
builder.Services.AddDbContext<SporContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Session (Oturum) Servisini Yapýlandýr
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 30 dakika hareketsizlikte oturum kapanýr
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // GDPR ve güvenlik için gerekli
});

// 4. HttpContextAccessor Ekle (Session'a her yerden eriþebilmek için)
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Hata Sayfasý Ayarlarý
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// --- KRÝTÝK SIRALAMA ---
// Session, her zaman Routing'den SONRA ama Authentication/Authorization'dan ÖNCE gelmelidir.
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();
// -----------------------

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"); // Varsayýlan açýlýþ Login olsun

app.Run();