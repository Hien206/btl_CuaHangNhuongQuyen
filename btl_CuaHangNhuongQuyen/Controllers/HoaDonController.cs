using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace btl_CuaHangNhuongQuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonBLL _hoaDonBLL;

        public HoaDonController(IHoaDonBLL hoaDonBLL)
        {
            _hoaDonBLL = hoaDonBLL;
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_hoaDonBLL.GetAll());
        }

        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(int id)
        {
            var res = _hoaDonBLL.GetById(id);
            if (res == null) return NotFound(new { message = "Không tìm thấy hóa đơn" });
            return Ok(res);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] HoaDonModel model)
        {
            var res = _hoaDonBLL.Create(model);
            return Ok(new { success = res });
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] HoaDonModel model)
        {
            var res = _hoaDonBLL.Update(model);
            return Ok(new { success = res });
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var res = _hoaDonBLL.Delete(id);
            return Ok(new { success = res });
        }
    }
}
