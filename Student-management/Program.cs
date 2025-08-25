// Program.cs

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Student_management.Models;

var builder = WebApplication.CreateBuilder(args);

// Lấy chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// **Đăng ký tất cả dịch vụ trước khi gọi builder.Build()**

builder.Services.AddDbContext<QuanLyHocSinhContext>(options =>
    options.UseSqlServer(connectionString));


// Cấu hình dịch vụ xác thực bằng cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
    });

// Thêm các dịch vụ vào container
builder.Services.AddControllersWithViews();

var app = builder.Build();

// **Sau khi app được build, cấu hình pipeline HTTP**

// Cấu hình pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Account/Login");
    // Thêm các middleware khác ở đây
}

// Thêm các middleware cần thiết
app.UseStaticFiles();
app.UseRouting();

// Sử dụng xác thực và phân quyền
app.UseAuthentication();
app.UseAuthorization();

// Thiết lập routing cho các Controller và Action
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();