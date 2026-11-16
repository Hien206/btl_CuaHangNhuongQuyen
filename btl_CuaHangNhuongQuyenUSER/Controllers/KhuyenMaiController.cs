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
    public class KhuyenMaiController : ControllerBase
    {
        private readonly IKhuyenMaiBLL _khuyenMaiBLL;

        public KhuyenMaiController(IKhuyenMaiBLL khuyenMaiBLL)
        {
            _khuyenMaiBLL = khuyenMaiBLL;
        }

        // Chủ cửa hàng tra cứu các chương trình khuyến mãi đang áp dụng
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_khuyenMaiBLL.GetAll());
        }

        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(int id)
        {
            var res = _khuyenMaiBLL.GetById(id);
            if (res == null) return NotFound(new { message = "Không tìm thấy khuyến mãi" });
            return Ok(res);
        }
    }
}
