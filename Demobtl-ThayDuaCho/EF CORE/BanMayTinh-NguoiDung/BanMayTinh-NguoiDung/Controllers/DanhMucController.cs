using BanMayTinh_NguoiDung.Code;
using BanMayTinh_NguoiDung.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanMayTinh_NguoiDung.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhMucController : ControllerBase
    {
        private BanMayTinhContext db = null;
        public DanhMucController(IConfiguration configuration)
        {
            db = new BanMayTinhContext(configuration);
        }
        [Route("get-nhasanxuat")]
        [HttpGet]
        public IActionResult GetByNhaSanXuat()
        {
            try
            {
                var list = db.NhaSanXuats.Select(
                    x => new { x.MaNhaSanXuat, x.TenNhaSanXuat }).ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [Route("get-donvitinh")]
        [HttpGet]
        public IActionResult GetByDonViTinh()
        {
            try
            {
                var list = db.DonViTinhs.Select(
                    x => new { x.MaDonViTinh, x.TenDonViTinh }).ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [Route("get-danhmuc")]
        [HttpGet]
        public IActionResult GetByDanhMuc()
        {
            try
            {
                var list = db.DanhMucs.Where(x => x.MaDanhMucCha == null).Select(
                    x => new { x.MaDanhMuc, x.TenDanhMuc }).ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Route("search")]
        [HttpPost]
        public ResponseModel Search([FromBody] Dictionary<string, object> formData)
        {
            var response = new ResponseModel();
            try
            {
                var page = int.Parse(formData["page"].ToString());
                var pageSize = int.Parse(formData["pageSize"].ToString());
                int? MaDanhMuc = null;
                if (formData.Keys.Contains("MaDanhMuc") && !string.IsNullOrEmpty(Convert.ToString(formData["MaDanhMuc"]))) { MaDanhMuc = Convert.ToInt32(formData["MaDanhMuc"].ToString()); }
                string TenSanPham = "";
                if (formData.Keys.Contains("TenSanPham") && !string.IsNullOrEmpty(Convert.ToString(formData["TenSanPham"]))) { TenSanPham = Convert.ToString(formData["TenSanPham"]); }
                var data = db.SanPhams.Where(x => (MaDanhMuc == null || x.MaDanhMuc == MaDanhMuc) && (TenSanPham == "" || x.TenSanPham.Contains(TenSanPham))).
                    Select(x => new { x.MaSanPham, x.TenSanPham, x.MoTaSanPham, x.ChiTietAnhSanPhams }).ToList();
                response.TotalItems = data.Count;
                response.Page = page;
                response.PageSize = pageSize;
                response.Data = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }

    }
}
