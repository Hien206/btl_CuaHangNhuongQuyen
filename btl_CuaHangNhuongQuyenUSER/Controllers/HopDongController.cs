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
    public class HopDongController : ControllerBase
    {
        private readonly IHopDongBLL _hopDongBLL;

        public HopDongController(IHopDongBLL hopDongBLL)
        {
            _hopDongBLL = hopDongBLL;
        }

        // Chủ cửa hàng chỉ có quyền xem hợp đồng đã ký
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_hopDongBLL.GetAll());
        }

        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(int id)
        {
            var res = _hopDongBLL.GetById(id);
            if (res == null) return NotFound(new { message = "Không tìm thấy hợp đồng" });
            return Ok(res);
        }
    }
}
