using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace btl_CuaHangNhuongQuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaiTroController : ControllerBase
    {
        private readonly IVaiTroBLL _vaiTroBLL;

        public VaiTroController(IVaiTroBLL vaiTroBLL)
        {
            _vaiTroBLL = vaiTroBLL;
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_vaiTroBLL.GetAll());
        }

        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(int id)
        {
            var res = _vaiTroBLL.GetById(id);
            if (res == null) return NotFound(new { message = "Không tìm thấy vai trò" });
            return Ok(res);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] VaiTroModel model)
        {
            var res = _vaiTroBLL.Create(model);
            return Ok(new { success = res });
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] VaiTroModel model)
        {
            var res = _vaiTroBLL.Update(model);
            return Ok(new { success = res });
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var res = _vaiTroBLL.Delete(id);
            return Ok(new { success = res });
        }
    }
}
