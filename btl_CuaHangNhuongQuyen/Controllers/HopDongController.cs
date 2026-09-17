using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace btl_CuaHangNhuongQuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HopDongController : ControllerBase
    {
        private readonly IHopDongBLL _hopDongBLL;

        public HopDongController(IHopDongBLL hopDongBLL)
        {
            _hopDongBLL = hopDongBLL;
        }

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

        [HttpPost("create")]
        public IActionResult Create([FromBody] HopDongModel model)
        {
            var res = _hopDongBLL.Create(model);
            return Ok(new { success = res });
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] HopDongModel model)
        {
            var res = _hopDongBLL.Update(model);
            return Ok(new { success = res });
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var res = _hopDongBLL.Delete(id);
            return Ok(new { success = res });
        }
    }
}
