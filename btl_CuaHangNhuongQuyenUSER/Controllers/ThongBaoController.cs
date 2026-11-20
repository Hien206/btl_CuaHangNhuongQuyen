using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace btl_CuaHangNhuongQuyenUSER.Controllers
{
    [Authorize(Roles = "CHU_CUA_HANG,ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class ThongBaoController : ControllerBase
    {
        private readonly IThongBaoBLL _thongBaoBLL;

        public ThongBaoController(IThongBaoBLL thongBaoBLL)
        {
            _thongBaoBLL = thongBaoBLL;
        }

        // Xem các thông báo gửi đến tài khoản chủ cửa hàng
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_thongBaoBLL.GetAll());
        }

        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(int id)
        {
            var res = _thongBaoBLL.GetById(id);
            if (res == null) return NotFound(new { message = "Không tìm thấy thông báo" });
            return Ok(res);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var res = _thongBaoBLL.Delete(id);
            return Ok(new { success = res });
        }

        [HttpDelete("cleanup-duplicates")]
        public IActionResult CleanupDuplicates()
        {
            int count = _thongBaoBLL.CleanupDuplicates();
            return Ok(new { success = true, deletedCount = count, message = $"Đã dọn dẹp {count} thông báo trùng lặp." });
        }

        [HttpDelete("delete-all")]
        public IActionResult DeleteAll()
        {
            var res = _thongBaoBLL.DeleteAll();
            return Ok(new { success = res });
        }
    }
}
