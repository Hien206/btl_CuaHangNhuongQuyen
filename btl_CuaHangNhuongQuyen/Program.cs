using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using DAL;
using DAL.Helper;
using DAL.Interfaces;
using BLL;
using BLL.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Register Database Helper
builder.Services.AddTransient<IDatabaseHelper, DatabaseHelper>();

// Register Repositories (DAL)
builder.Services.AddTransient<IVaiTroRepository, VaiTroRepository>();
builder.Services.AddTransient<INguoiDungRepository, NguoiDungRepository>();
builder.Services.AddTransient<ICuaHangRepository, CuaHangRepository>();
builder.Services.AddTransient<IHopDongRepository, HopDongRepository>();
builder.Services.AddTransient<IDoanhThuRepository, DoanhThuRepository>();
builder.Services.AddTransient<IKhuyenMaiRepository, KhuyenMaiRepository>();
builder.Services.AddTransient<IHoaDonRepository, HoaDonRepository>();
builder.Services.AddTransient<IThanhToanRepository, ThanhToanRepository>();
builder.Services.AddTransient<IThongBaoRepository, ThongBaoRepository>();
builder.Services.AddTransient<ILichSuThaoTacRepository, LichSuThaoTacRepository>();

// Register Business Logic Services (BLL)
builder.Services.AddTransient<IVaiTroBLL, VaiTroBLL>();
builder.Services.AddTransient<INguoiDungBLL, NguoiDungBLL>();
builder.Services.AddTransient<ICuaHangBLL, CuaHangBLL>();
builder.Services.AddTransient<IHopDongBLL, HopDongBLL>();
builder.Services.AddTransient<IDoanhThuBLL, DoanhThuBLL>();
builder.Services.AddTransient<IKhuyenMaiBLL, KhuyenMaiBLL>();
builder.Services.AddTransient<IHoaDonBLL, HoaDonBLL>();
builder.Services.AddTransient<IThanhToanBLL, ThanhToanBLL>();
builder.Services.AddTransient<IThongBaoBLL, ThongBaoBLL>();
builder.Services.AddTransient<ILichSuThaoTacBLL, LichSuThaoTacBLL>();

// JWT Authentication Configuration
var secretKey = builder.Configuration["Jwt:Key"];
var keyBytes = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BTL Chuỗi Cửa Hàng Nhượng Quyền API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập Token theo định dạng: Bearer {token_cua_ban}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
