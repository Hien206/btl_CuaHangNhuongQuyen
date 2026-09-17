using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace btl_CuaHangNhuongQuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiController : ControllerBase
    {
        private readonly IKhuyenMaiBLL _khuyenMaiBLL;

        public KhuyenMaiController(IKhuyenMaiBLL khuyenMaiBLL)
        {
            _khuyenMaiBLL = khuyenMaiBLL;
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_khuyenMaiBLL.GetAll());
        }

        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(int id)
        {
            var res = _khuyenMaiBLL.GetById(id);
            if (res == null) return NotFound(new { message = "Không tìm thấy chương trình khuyến mãi" });
            return Ok(res);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] KhuyenMaiModel model)
        {
            var res = _khuyenMaiBLL.Create(model);
            return Ok(new { success = res });
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] KhuyenMaiModel model)
        {
            var res = _khuyenMaiBLL.Update(model);
            return Ok(new { success = res });
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var res = _khuyenMaiBLL.Delete(id);
            return Ok(new { success = res });
        }
    }
}
