using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace btl_CuaHangNhuongQuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanhToanController : ControllerBase
    {
        private readonly IThanhToanBLL _thanhToanBLL;

        public ThanhToanController(IThanhToanBLL thanhToanBLL)
        {
            _thanhToanBLL = thanhToanBLL;
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_thanhToanBLL.GetAll());
        }

        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(int id)
        {
            var res = _thanhToanBLL.GetById(id);
            if (res == null) return NotFound(new { message = "Không tìm thấy giao dịch thanh toán" });
            return Ok(res);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] ThanhToanModel model)
        {
            var res = _thanhToanBLL.Create(model);
            return Ok(new { success = res });
        }
    }
}
