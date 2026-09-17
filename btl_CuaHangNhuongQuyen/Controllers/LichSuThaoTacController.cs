using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace btl_CuaHangNhuongQuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LichSuThaoTacController : ControllerBase
    {
        private readonly ILichSuThaoTacBLL _lichSuThaoTacBLL;

        public LichSuThaoTacController(ILichSuThaoTacBLL lichSuThaoTacBLL)
        {
            _lichSuThaoTacBLL = lichSuThaoTacBLL;
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_lichSuThaoTacBLL.GetAll());
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] LichSuThaoTacModel model)
        {
            var res = _lichSuThaoTacBLL.Create(model);
            return Ok(new { success = res });
        }
    }
}
