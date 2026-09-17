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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
