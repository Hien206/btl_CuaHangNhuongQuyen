using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BLL.Interfaces;
using Model;

namespace btl_CuaHangNhuongQuyen.Services
{
    public class HopDongExpirationBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<HopDongExpirationBackgroundService> _logger;

        public HopDongExpirationBackgroundService(IServiceScopeFactory scopeFactory, ILogger<HopDongExpirationBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("--> HopDongExpirationBackgroundService đã khởi động.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var hopDongBLL = scope.ServiceProvider.GetRequiredService<IHopDongBLL>();
                        var cuaHangBLL = scope.ServiceProvider.GetRequiredService<ICuaHangBLL>();
                        var doanhThuBLL = scope.ServiceProvider.GetRequiredService<IDoanhThuBLL>();
                        var thongBaoBLL = scope.ServiceProvider.GetRequiredService<IThongBaoBLL>();

                        var today = DateTime.Now.Date;
                        var yesterday = today.AddDays(-1);

                        // 1. Quét Hợp đồng sắp hết hạn (còn dưới 30 ngày)
                        var listHopDong = hopDongBLL.GetAll();
                        if (listHopDong != null)
                        {
                            foreach (var hd in listHopDong)
                            {
                                if (hd.trangthai == "hieuluc" || string.IsNullOrEmpty(hd.trangthai))
                                {
                                    double remainingDays = (hd.ngayketthuc.Date - today).TotalDays;
                                    if (remainingDays >= 0 && remainingDays <= 30)
                                    {
                                        var ch = cuaHangBLL.GetById(hd.cuahangid);
                                        int targetUserId = (ch != null && ch.chu_cua_hang_id > 0) ? ch.chu_cua_hang_id : 1;

                                        thongBaoBLL.Create(new ThongBaoModel
                                        {
                                            nguoidungid = targetUserId,
                                            loaithongbao = "SANG_HET_HAN_HOP_DONG",
                                            noidung = $"[CẢNH BÁO HỢP ĐỒNG] Hợp đồng #{hd.sohopdong} của cửa hàng {(ch != null ? ch.tencuahang : "ID " + hd.cuahangid)} sắp hết hạn vào ngày {hd.ngayketthuc:dd/MM/yyyy} (Còn {Math.Ceiling(remainingDays)} ngày).",
                                            dadoc = false,
                                            ngaytao = DateTime.Now
                                        });
                                    }
                                }
                            }
                        }

                        // 2. Quét Cửa hàng chưa nộp báo cáo doanh thu ngày hôm qua
                        var listCuaHang = cuaHangBLL.GetAll();
                        var listDoanhThu = doanhThuBLL.GetAll();

                        if (listCuaHang != null)
                        {
                            foreach (var ch in listCuaHang)
                            {
                                if (ch.trangthai == "hoatdong" || string.IsNullOrEmpty(ch.trangthai))
                                {
                                    bool daNopBaoCao = listDoanhThu != null && listDoanhThu.Any(dt => dt.cuahangid == ch.id && dt.ngaybaocao.Date == yesterday);
                                    if (!daNopBaoCao)
                                    {
                                        int targetUserId = ch.chu_cua_hang_id > 0 ? ch.chu_cua_hang_id : 1;
                                        thongBaoBLL.Create(new ThongBaoModel
                                        {
                                            nguoidungid = targetUserId,
                                            loaithongbao = "CHAM_BAO_CAO",
                                            noidung = $"[CẢNH BÁO DOANH THU] Cửa hàng {ch.tencuahang} (Mã: {ch.macuahang}) chưa nộp báo cáo doanh thu ngày {yesterday:dd/MM/yyyy}.",
                                            dadoc = false,
                                            ngaytao = DateTime.Now
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi xảy ra trong tiến trình HopDongExpirationBackgroundService.");
                }

                // Tự động lặp lại sau mỗi 1 phút (phục vụ demo cho giáo viên)
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
