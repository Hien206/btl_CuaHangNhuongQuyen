using BanMayTinh_Admin.Code;
using BanMayTinh_Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace BanMayTinh_Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private BanMayTinhContext db = null;
        private ITools _tools;
        public UsersController(ITools tools, IConfiguration configuration)
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
                var query = from n in db.NguoiDungs
                            join t in db.TaiKhoans on n.MaNguoiDung equals t.MaNguoiDung
                            select new
                            {
                                MaNguoiDung = n.MaNguoiDung,
                                HoTen = n.HoTen,
                                DienThoai = n.DienThoai,
                                TaiKhoan = t.TaiKhoan1,
                                MatKhau = t.MatKhau
                            };
                var result = query.SingleOrDefault(x => x.MaNguoiDung == id);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok("Err");
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

        [Route("create-user")]
        [HttpPost]
        public IActionResult CreateUser(UserModels model)
        {
            try
            {
                var user = db.TaiKhoans.SingleOrDefault(x => x.TaiKhoan1 == model.taikhoan.TaiKhoan1);
                if (user == null)
                {
                    db.NguoiDungs.Add(model.nguoidung);
                    db.SaveChanges();
                    model.taikhoan.MaNguoiDung = model.nguoidung.MaNguoiDung;
                    db.TaiKhoans.Add(model.taikhoan);
                    db.SaveChanges();
                    return Ok("OK");
                }
                else
                {
                    return Ok("Tài khoản đã tồn tại!");
                }

            }
            catch (Exception ex)
            {
                return Ok("Err");
            }
        }

        [Route("update-user")]
        [HttpPost]
        public IActionResult UpdateUser(UserEditModels model)
        {
            try
            {
                var nguoidung = db.NguoiDungs.SingleOrDefault(x => x.MaNguoiDung == model.MaNguoiDung);
                if (nguoidung != null)
                {
                    nguoidung.HoTen = string.IsNullOrEmpty(model.HoTen) ? nguoidung.HoTen : model.HoTen;
                    nguoidung.DiaChi = string.IsNullOrEmpty(model.DiaChi) ? nguoidung.DiaChi : model.DiaChi;
                    //....
                }
                var taikhoan = db.TaiKhoans.SingleOrDefault(x => x.MaNguoiDung == model.MaNguoiDung);
                if (nguoidung != null)
                {
                    taikhoan.MatKhau = string.IsNullOrEmpty(model.MatKhau) ? taikhoan.MatKhau : model.MatKhau;
                    //....
                }
                db.SaveChanges();
                return Ok("OK");
            }
            catch (Exception ex)
            {
                return Ok("Err");
            }
        }

        [Route("delete-user")]
        [HttpDelete]
        public IActionResult DeleteUser(int MaNguoiDung)
        {
            try
            {
                var taikhoan = db.TaiKhoans.SingleOrDefault(x => x.MaNguoiDung == MaNguoiDung);
                db.TaiKhoans.Remove(taikhoan);
                db.SaveChanges();
                var nguoidung = db.NguoiDungs.SingleOrDefault(x => x.MaNguoiDung == MaNguoiDung);
                db.NguoiDungs.Remove(nguoidung);
                db.SaveChanges();
                return Ok("OK");
            }
            catch (Exception ex)
            {
                return Ok("Err");
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

                string HoTen = "";
                if (formData.Keys.Contains("HoTen") && !string.IsNullOrEmpty(Convert.ToString(formData["HoTen"]))) { HoTen = Convert.ToString(formData["HoTen"]); }
                string DienThoai = "";
                if (formData.Keys.Contains("DienThoai") && !string.IsNullOrEmpty(Convert.ToString(formData["DienThoai"]))) { HoTen = Convert.ToString(formData["DienThoai"]); }

                DateTime? fr_NgaySinh = null;
                if (formData.Keys.Contains("fr_NgaySinh") && formData["fr_NgaySinh"] != null && formData["fr_NgaySinh"].ToString() != "")
                {
                    var dt = Convert.ToDateTime(formData["fr_NgaySinh"].ToString());
                    fr_NgaySinh = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
                }
                DateTime? to_NgaySinh = null;
                if (formData.Keys.Contains("to_NgaySinh") && formData["to_NgaySinh"] != null && formData["to_NgaySinh"].ToString() != "")
                {
                    var dt = Convert.ToDateTime(formData["to_NgaySinh"].ToString());
                    to_NgaySinh = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, 999);
                }

                string TaiKhoan = "";
                if (formData.Keys.Contains("TaiKhoan") && !string.IsNullOrEmpty(Convert.ToString(formData["TaiKhoan"]))) { HoTen = Convert.ToString(formData["TaiKhoan"]); }

                var query = from n in db.NguoiDungs
                            join t in db.TaiKhoans on n.MaNguoiDung equals t.MaNguoiDung
                            select new
                            {
                                MaNguoiDung = n.MaNguoiDung,
                                HoTen = n.HoTen,
                                NgaySinh = n.NgaySinh,
                                DienThoai = n.DienThoai,
                                TaiKhoan = t.TaiKhoan1,
                                MatKhau = t.MatKhau
                            };
                var data = query.Where(x =>
                (HoTen == "" || x.HoTen.Contains(HoTen)) &&
                (DienThoai == "" || x.DienThoai.Contains(DienThoai)) &&
                (TaiKhoan == "" || x.TaiKhoan == TaiKhoan) &&
                (
                  (fr_NgaySinh == null && to_NgaySinh == null) ||
                  (fr_NgaySinh != null && to_NgaySinh == null && x.NgaySinh >= fr_NgaySinh) ||
                  (fr_NgaySinh == null && to_NgaySinh != null && x.NgaySinh <= to_NgaySinh) ||
                  (x.NgaySinh >= fr_NgaySinh && x.NgaySinh <= to_NgaySinh)
                )
                ).ToList();
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
