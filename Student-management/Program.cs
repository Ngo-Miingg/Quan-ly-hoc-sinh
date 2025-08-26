using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Student_Management.Models;

var builder = WebApplication.CreateBuilder(args);

// === ĐĂNG KÝ CÁC DỊCH VỤ (SERVICES) ===

// 1. Đăng ký dịch vụ MVC
builder.Services.AddControllersWithViews();

// 2. Đăng ký DbContext
// Lấy chuỗi kết nối từ appsettings.json với đúng tên là "DefaultConnection"
builder.Services.AddDbContext<QuanLyHocSinhContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Đăng ký Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tùy chỉnh thời gian timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 4. Đăng ký Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

// 5. Đăng ký Authorization (phân quyền)
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"))
    .AddPolicy("RequireGiaoVien", policy => policy.RequireRole("GiaoVien"))
    .AddPolicy("RequireHocSinh", policy => policy.RequireRole("HocSinh"));


// ✅ Build ứng dụng sau khi đã đăng ký xong tất cả dịch vụ
var app = builder.Build();

// === CẤU HÌNH MIDDLEWARE PIPELINE (Thứ tự rất quan trọng) ===

// 6. Cấu hình cho môi trường Production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Bật session trước khi xác thực và phân quyền
app.UseSession();

// Bật xác thực và phân quyền
app.UseAuthentication();
app.UseAuthorization();

// 7. Map các route cho controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();