using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Student_Management.Models;
using System; // Thêm using cho Exception

var builder = WebApplication.CreateBuilder(args);

// 1. Đăng ký các dịch vụ
builder.Services.AddControllersWithViews();

// 2. Đăng ký DbContext (Đoạn này rất quan trọng)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<QuanLyHocSinhContext>(options =>
    options.UseSqlServer(connectionString));

// ... các dịch vụ Authentication, Authorization ...
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

// ==========================================================
// === ĐOẠN CODE KIỂM TRA KẾT NỐI DATABASE TRỰC TIẾP ===
// ==========================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<QuanLyHocSinhContext>();
        // Thử thực hiện một truy vấn đơn giản nhất
        var namHocCount = dbContext.NamHocs.Count();
        // Nếu không có lỗi, in ra thông báo thành công
        Console.WriteLine($"==========================================================");
        Console.WriteLine($"KET NOI DATABASE THANH CONG! So luong Nam Hoc: {namHocCount}");
        Console.WriteLine($"==========================================================");
    }
    catch (Exception ex)
    {
        // Nếu có lỗi, in ra lỗi chi tiết
        Console.WriteLine($"==========================================================");
        Console.WriteLine($"!!! KET NOI DATABASE THAT BAI: {ex.Message}");
        Console.WriteLine($"==========================================================");
    }
}
// ==========================================================

// Cấu hình HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();