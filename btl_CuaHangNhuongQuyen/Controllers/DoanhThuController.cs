using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace btl_CuaHangNhuongQuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoanhThuController : ControllerBase
    {
        private readonly IDoanhThuBLL _doanhThuBLL;

        public DoanhThuController(IDoanhThuBLL doanhThuBLL)
        {
            _doanhThuBLL = doanhThuBLL;
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_doanhThuBLL.GetAll());
        }

        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(int id)
        {
            var res = _doanhThuBLL.GetById(id);
            if (res == null) return NotFound(new { message = "Không tìm thấy doanh thu" });
            return Ok(res);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] DoanhThuModel model)
        {
            var res = _doanhThuBLL.Create(model);
            return Ok(new { success = res });
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] DoanhThuModel model)
        {
            var res = _doanhThuBLL.Update(model);
            return Ok(new { success = res });
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var res = _doanhThuBLL.Delete(id);
            return Ok(new { success = res });
        }
    }
}
