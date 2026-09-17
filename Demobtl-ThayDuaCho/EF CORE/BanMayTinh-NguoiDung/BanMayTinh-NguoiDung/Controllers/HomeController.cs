using BanMayTinh_NguoiDung.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanMayTinh_NguoiDung.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private BanMayTinhContext db = null;
        public HomeController(IConfiguration configuration)
        {
            db = new BanMayTinhContext(configuration);
        }
        [Route("get-banchay/{sl}")]
        [HttpGet]
        public IActionResult SanPhamBanChay(int sl)
        {
            var query = from c in db.ChiTietHoaDonXuats
                        join s in db.SanPhams on c.MaSanPham equals s.MaSanPham
                        group c by new { MaSanPham = s.MaSanPham, TenSanPham = s.TenSanPham, AnhDaiDien = string.IsNullOrEmpty(s.AnhDaiDien) ? "" : s.AnhDaiDien } into g
                        select new
                        {
                            MaSanPham = g.Key.MaSanPham,
                            TenSanPham = g.Key.TenSanPham,
                            AnhDaiDien = g.Key.AnhDaiDien,
                            Tong = g.Sum(x => (x.SoLuong))
                        };
            var result = query.OrderByDescending(x => x.Tong).Take(sl).ToList();
            return Ok(new { result });
        }
        [Route("get-dat/{sl}")]
        [HttpGet]
        public IActionResult SanPhamDat(int sl)
        {
            var query = from c in db.ChiTietDonHangs
                        join s in db.SanPhams on c.MaSanPham equals s.MaSanPham
                        group c by new { MaSanPham = s.MaSanPham, TenSanPham = s.TenSanPham, AnhDaiDien = string.IsNullOrEmpty(s.AnhDaiDien) ? "" : s.AnhDaiDien } into g
                        select new
                        {
                            MaSanPham = g.Key.MaSanPham,
                            TenSanPham = g.Key.TenSanPham,
                            AnhDaiDien = g.Key.AnhDaiDien,
                            Tong = g.Sum(x => (x.SoLuong))
                        };
            var result = query.OrderByDescending(x => x.Tong).Take(sl).ToList();
            return Ok(result);
        }
        [Route("get-moi/{sl}")]
        [HttpGet]
        public IActionResult SanPhamMoi(int sl)
        {
            var result = db.SanPhams.Select(x => new { x.MaDanhMuc, x.TenSanPham, x.MaSanPham, x.NgayTao }).OrderByDescending(x => x.NgayTao).Take(sl).ToList();
            return Ok(result);
        }

        [Route("get-home/{sl}")]
        [HttpGet]
        public IActionResult SanPhamHome(int sl)
        {


            var query1 = from c in db.ChiTietDonHangs
                         join s in db.SanPhams on c.MaSanPham equals s.MaSanPham
                         group c by new { MaSanPham = s.MaSanPham, TenSanPham = s.TenSanPham, AnhDaiDien = string.IsNullOrEmpty(s.AnhDaiDien) ? "" : s.AnhDaiDien } into g
                         select new
                         {
                             MaSanPham = g.Key.MaSanPham,
                             TenSanPham = g.Key.TenSanPham,
                             AnhDaiDien = g.Key.AnhDaiDien,
                             Tong = g.Sum(x => (x.SoLuong))
                         };
            var result1 = query1.OrderByDescending(x => x.Tong).Take(sl).ToList();



            var query2 = from c in db.ChiTietDonHangs
                         join s in db.SanPhams on c.MaSanPham equals s.MaSanPham
                         group c by new { MaSanPham = s.MaSanPham, TenSanPham = s.TenSanPham, AnhDaiDien = string.IsNullOrEmpty(s.AnhDaiDien) ? "" : s.AnhDaiDien } into g
                         select new
                         {
                             MaSanPham = g.Key.MaSanPham,
                             TenSanPham = g.Key.TenSanPham,
                             AnhDaiDien = g.Key.AnhDaiDien,
                             Tong = g.Sum(x => (x.SoLuong))
                         };
            var result2 = query2.OrderByDescending(x => x.Tong).Take(sl).ToList();

            var result3 = db.SanPhams.Select(x => new { x.TenSanPham, x.MaSanPham, x.NgayTao }).OrderByDescending(x => x.NgayTao).Take(sl).ToList();
            return Ok(new
            {
                listbanchay = result1,
                listnhieunguoidat = result2,
                listmoive = result3
            }); ;
        }
    }
}
