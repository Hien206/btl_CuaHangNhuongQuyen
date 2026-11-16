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
    public class CuaHangController : ControllerBase
    {
        private readonly ICuaHangBLL _cuaHangBLL;

        public CuaHangController(ICuaHangBLL cuaHangBLL)
        {
            _cuaHangBLL = cuaHangBLL;
        }

        // Chủ cửa hàng chỉ có quyền xem thông tin chi nhánh
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_cuaHangBLL.GetAll());
        }

        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(int id)
        {
            var res = _cuaHangBLL.GetById(id);
            if (res == null) return NotFound(new { message = "Không tìm thấy cửa hàng" });
            return Ok(res);
        }
    }
}
