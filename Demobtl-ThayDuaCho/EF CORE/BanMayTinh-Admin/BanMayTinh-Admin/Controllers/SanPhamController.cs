using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using BanMayTinh_Admin.Models;
using BanMayTinh_Admin.Code;

namespace BanMayTinh_Admin.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class SanPhamController : ControllerBase
    {
        private BanMayTinhContext db = null;
        private ITools _tools;
        public SanPhamController(ITools tools, IConfiguration configuration)
        {
            _tools = tools;
            db = new BanMayTinhContext(configuration);
        }
        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            try
            {
                var sp = db.SanPhams.Where(x => x.MaSanPham == id).Select(
                    x => new { x.MaSanPham, x.TenSanPham, x.MaDanhMuc, x.MaNhaSanXuat, x.AnhDaiDien, x.MaDonViTinh, x.MoTaSanPham }).SingleOrDefault();
                return Ok(sp);

            }
            catch
            {
                return BadRequest();
            }
        }
        [Route("create-sanpham")]
        [HttpPost]
        public IActionResult CreateSanPham(SanPhamModels model)
        {
            try
            {
                model.sanpham.NgayTao = DateTime.Now;
                db.SanPhams.Add(model.sanpham);
                db.SaveChanges();

                var g = new GiaSanPham();
                g.MaSanPham = model.sanpham.MaSanPham;
                g.NgayBatDau = DateTime.Now;
                g.Gia = 100;
                db.GiaSanPhams.Add(g);
                db.SaveChanges();

                if (model.listchitiet != null && model.listchitiet.Count > 0)
                {
                    foreach (var x in model.listchitiet)
                        x.MaSanPham = model.sanpham.MaSanPham;
                    model.sanpham.ChiTietAnhSanPhams = model.listchitiet;
                    db.SaveChanges();
                }
                return Ok("OK");
            }
            catch
            {
                return BadRequest();
            }
        }
        [Route("update-sanpham")]
        [HttpPost]
        public IActionResult UpdateSanPham(SanPhamEditModels model)
        {
            try
            {
                var sp = db.SanPhams.SingleOrDefault(x => x.MaSanPham == model.sanpham.MaSanPham);
                sp.TenSanPham = string.IsNullOrEmpty(model.sanpham.TenSanPham) ? sp.TenSanPham : model.sanpham.TenSanPham;
                sp.MoTaSanPham = model.sanpham.MoTaSanPham;
                sp.AnhDaiDien = model.sanpham.AnhDaiDien;
                db.SaveChanges();
                if (model.listchitiet != null && model.listchitiet.Count > 0)
                {
                    foreach (var x in model.listchitiet)
                    {
                        if (x.TrangThai == 1)
                        {
                            var anh = new ChiTietAnhSanPham();
                            anh.Anh = x.Anh;
                            anh.MaSanPham = sp.MaSanPham;
                            db.ChiTietAnhSanPhams.Add(anh);
                            db.SaveChanges();
                        }
                        else if (x.TrangThai == 0)
                        {
                            var obj = db.ChiTietAnhSanPhams.SingleOrDefault(s => s.MaAnhChitiet == x.MaAnhChitiet);
                            db.ChiTietAnhSanPhams.Remove(obj);
                            db.SaveChanges();
                        }
                    }

                }
                return Ok("OK");
            }
            catch
            {
                return BadRequest();
            }
        }

        [Route("delete-sanpham/{MaSanPham}")]
        [HttpGet]
        public IActionResult DeleteSanPham(int MaSanPham)
        {
            try
            {
                var listanh = db.ChiTietAnhSanPhams.Where(x => x.MaSanPham == MaSanPham).ToList();
                db.ChiTietAnhSanPhams.RemoveRange(listanh);
                db.SaveChanges();
                var listgia = db.GiaSanPhams.Where(x => x.MaSanPham == MaSanPham).ToList();
                db.GiaSanPhams.RemoveRange(listgia);
                db.SaveChanges();
                var sp = db.SanPhams.SingleOrDefault(x => x.MaSanPham == MaSanPham);
                db.SanPhams.Remove(sp);
                db.SaveChanges();
                return Ok("OK");
            }
            catch
            {
                return BadRequest();
            }
        }

        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    string filePath = $"upload/{file.FileName.Replace("-", "_").Replace("%", "")}";
                    var fullPath = _tools.CreatePathFile(filePath);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return Ok(new { filePath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Không thể upload tệp");
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
                var query = from s in db.SanPhams
                            join g in db.GiaSanPhams on s.MaSanPham equals g.MaSanPham
                            join n in db.NhaSanXuats on s.MaNhaSanXuat equals n.MaNhaSanXuat
                            join d in db.DonViTinhs on s.MaDonViTinh equals d.MaDonViTinh
                            select new
                            {
                                s.MaSanPham,
                                s.TenSanPham,
                                s.MoTaSanPham,
                                g.Gia,
                                n.TenNhaSanXuat,
                                d.TenDonViTinh,
                                s.MaDanhMuc
                            };
                var data = query.Where(x => (MaDanhMuc == null || x.MaDanhMuc == MaDanhMuc) && (TenSanPham == "" || x.TenSanPham.Contains(TenSanPham))).OrderByDescending(x=>x.MaSanPham).ToList();
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