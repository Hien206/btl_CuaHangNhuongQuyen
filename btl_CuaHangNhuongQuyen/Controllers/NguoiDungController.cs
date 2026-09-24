using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BLL.Interfaces;
using Model;

namespace btl_CuaHangNhuongQuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {
        private readonly INguoiDungBLL _nguoiDungBLL;
        private readonly IConfiguration _config;

        public NguoiDungController(INguoiDungBLL nguoiDungBLL, IConfiguration config)
        {
            _nguoiDungBLL = nguoiDungBLL;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            if (model == null || string.IsNullOrEmpty(model.email) || string.IsNullOrEmpty(model.matkhau))
            {
                return BadRequest(new { message = "Email và mật khẩu không được để trống" });
            }

            var user = _nguoiDungBLL.Login(model.email, model.matkhau);
            if (user == null)
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không chính xác" });
            }

            string secretKey = _config["Jwt:Key"];
            string issuer = _config["Jwt:Issuer"];
            string audience = _config["Jwt:Audience"];

            var token = _nguoiDungBLL.GenerateJwtToken(user, secretKey, issuer, audience);

            return Ok(new
            {
                message = "Đăng nhập thành công",
                token = token,
                user = new
                {
                    user.id,
                    user.email,
                    user.hoten,
                    user.vaitroid
                }
            });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest model)
        {
            if (model == null || string.IsNullOrEmpty(model.email) || string.IsNullOrEmpty(model.matkhau))
            {
                return BadRequest(new { message = "Email và mật khẩu không được để trống" });
            }

            var res = _nguoiDungBLL.Register(model.email, model.matkhau, model.hoten, model.vaitroid);
            if (!res)
            {
                return BadRequest(new { message = "Tạo tài khoản thất bại (có thể email đã tồn tại)" });
            }

            return Ok(new { message = "Tạo tài khoản thành công!" });
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_nguoiDungBLL.GetAll());
        }

        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(int id)
        {
            var user = _nguoiDungBLL.GetById(id);
            if (user == null) return NotFound(new { message = "Không tìm thấy người dùng" });
            return Ok(user);
        }
    }

    public class LoginRequest
    {
        public string email { get; set; }
        public string matkhau { get; set; }
    }

    public class RegisterRequest
    {
        public string email { get; set; }
        public string matkhau { get; set; }
        public string hoten { get; set; }
        public int vaitroid { get; set; }
    }
}
