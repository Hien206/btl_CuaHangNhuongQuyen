using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 1. Nạp file cấu hình ocelot.json
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// 2. Thêm dịch vụ Ocelot
builder.Services.AddOcelot(builder.Configuration);

// 3. Mở CORS để Web Front-end gọi vào Gateway không bị chặn
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Kích hoạt CORS
app.UseCors("AllowAll");

// Trang chủ hiển thị trạng thái của Gateway khi mở trên trình duyệt
app.MapGet("/", () => Results.Ok(new
{
    status = "Online",
    gateway = "Franchise API Gateway (Ocelot)",
    description = "Hệ thống Cổng trung chuyển dịch vụ nhượng quyền",
    routes = new
    {
        adminService = "/api/admin/*  ==>  https://localhost:7134/api/*",
        userService  = "/api/user/*   ==>  https://localhost:7005/api/*",
        defaultRoute = "/api/*        ==>  https://localhost:7134/api/*"
    }
}));

// Kích hoạt Ocelot Middleware
await app.UseOcelot();

app.Run();
