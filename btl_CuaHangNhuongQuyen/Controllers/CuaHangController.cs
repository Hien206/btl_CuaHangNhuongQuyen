using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace btl_CuaHangNhuongQuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuaHangController : ControllerBase
    {
        private readonly ICuaHangBLL _cuaHangBLL;

        public CuaHangController(ICuaHangBLL cuaHangBLL)
        {
            _cuaHangBLL = cuaHangBLL;
        }

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

        [HttpPost("create")]
        public IActionResult Create([FromBody] CuaHangModel model)
        {
            var res = _cuaHangBLL.Create(model);
            return Ok(new { success = res });
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] CuaHangModel model)
        {
            var res = _cuaHangBLL.Update(model);
            return Ok(new { success = res });
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var res = _cuaHangBLL.Delete(id);
            return Ok(new { success = res });
        }
    }
}
