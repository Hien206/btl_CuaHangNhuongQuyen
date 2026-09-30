using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Model;
using Asp.Versioning;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserBusiness _userBusiness;
        private string _path;
        private IWebHostEnvironment _env;
        private readonly ILogger<UsersController> _logger;
        public UsersController(
            IUserBusiness userBusiness,
            IConfiguration configuration,
            IWebHostEnvironment env,
            ILogger<UsersController> logger)
        {
            _userBusiness = userBusiness;
            _path = configuration["AppSettings:PATH"];
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthenticateModel model)
        {
            var user = _userBusiness.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(new { user_id = user.user_id, hoten = user.hoten, taikhoan = user.taikhoan, token = user.token });
        }


        [NonAction]
        public string SaveFileFromBase64String(string RelativePathFileName, string dataFromBase64String)
        {
            if (dataFromBase64String.Contains("base64,"))
            {
                dataFromBase64String = dataFromBase64String.Substring(dataFromBase64String.IndexOf("base64,", 0) + 7);
            }
            return WriteFileToAuthAccessFolder(RelativePathFileName, dataFromBase64String);
        }

        [NonAction]
        public string WriteFileToAuthAccessFolder(string RelativePathFileName, string base64StringData)
        {
            string serverRootPathFolder = _path;
            string fullPathFile = $@"{serverRootPathFolder}\{RelativePathFileName}";
            string fullPathFolder = System.IO.Path.GetDirectoryName(fullPathFile);
            if (!Directory.Exists(fullPathFolder))
                Directory.CreateDirectory(fullPathFolder);
            System.IO.File.WriteAllBytes(fullPathFile, Convert.FromBase64String(base64StringData));
            return string.Empty;
        }

        [Route("delete-user")]
        [HttpPost]
        public IActionResult DeleteUser([FromBody] Dictionary<string, object> formData)
        {
            string user_id = "";
            if (formData.Keys.Contains("user_id") && !string.IsNullOrEmpty(Convert.ToString(formData["user_id"]))) { user_id = Convert.ToString(formData["user_id"]); }
            _userBusiness.Delete(user_id);
            return Ok();
        }

        [Route("download/{filename}")]
        [HttpGet]
        public IActionResult DownloadSetupSataProduct(string filename)
        {
            var safeFilename = Path.GetFileName(filename);
            var filePath = Path.Combine(_env.ContentRootPath, "upload", safeFilename);
            if (!System.IO.File.Exists(filePath))
                throw new KeyNotFoundException($"Không tìm thấy tệp '{safeFilename}'.");

            _logger.LogInformation("Người dùng tải tệp {FileName}", safeFilename);
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(stream, "application/octet-stream", safeFilename);
        }

        [Route("upload")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file is null || file.Length <= 0)
            {
                throw new ArgumentException("Tệp tải lên không được để trống.", nameof(file));
            }

            var safeFilename = Path.GetFileName(file.FileName);
            string filePath = $"upload/{safeFilename}";
            var fullPath = CreatePathFile(filePath);
            await using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            _logger.LogInformation(
                "Đã tải lên tệp {FileName}, kích thước {FileSize} byte",
                safeFilename,
                file.Length);
            return Ok(new { filePath });
        }

        [NonAction]
        private string CreatePathFile(string RelativePathFileName)
        {
            string serverRootPathFolder = _path;
            string fullPathFile = $@"{serverRootPathFolder}\{RelativePathFileName}";
            string fullPathFolder = System.IO.Path.GetDirectoryName(fullPathFile);
            if (!Directory.Exists(fullPathFolder))
                Directory.CreateDirectory(fullPathFolder);
            return fullPathFile;
        }

        [Route("create-user1")]
        [HttpPost]
        public UserModel CreateUser1([FromBody] UserModel model)
        {
            model.user_id = Guid.NewGuid().ToString();
            _userBusiness.Create(model);
            return model;
        }


        [Route("create-user2")]
        [HttpPost]
        public async Task<UserModel> CreateUser2([FromForm] IFormFile file, [FromForm] string? hoten, [FromForm] DateTime? ngaysinh, [FromForm] string? taikhoan, [FromForm] string? matkhau)
        {
            if (file.Length > 0)
            {
                string filePath = $"upload/{file.FileName}";
                var fullPath = CreatePathFile(filePath);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                var model = new UserModel() { hoten = hoten, ngaysinh = ngaysinh, taikhoan = taikhoan, matkhau = matkhau };
                model.user_id = Guid.NewGuid().ToString();
                model.image_url = filePath;
                _userBusiness.Create(model);
                return model;
            }
            else
            {
                var model = new UserModel() { hoten = hoten, ngaysinh = ngaysinh, taikhoan = taikhoan, matkhau = matkhau };
                model.user_id = Guid.NewGuid().ToString();
                model.image_url = null;
                _userBusiness.Create(model);
                return model;
            }
        }

        [Route("update-user")]
        [HttpPost]
        public UserModel UpdateUser([FromBody] UserModel model)
        {
            if (model.image_url != null)
            {
                var arrData = model.image_url.Split(';');
                if (arrData.Length == 3)
                {
                    var savePath = $@"assets/images/{arrData[0]}";
                    model.image_url = $"{savePath}";
                    SaveFileFromBase64String(savePath, arrData[2]);
                }
            }
            _userBusiness.Update(model);
            return model;
        }

        [Route("get-by-id/{id}")]
        [HttpGet]
        public UserModel GetDatabyID(string id)
        {
            return _userBusiness.GetDatabyID(id);
        }

        [Route("search")]
        [HttpPost]
        public ResponseModel Search([FromBody] Dictionary<string, object> formData)
        {
            var response = new ResponseModel();
            var page = int.Parse(formData["page"].ToString());
            var pageSize = int.Parse(formData["pageSize"].ToString());
            string hoten = "";
            if (formData.Keys.Contains("hoten") && !string.IsNullOrEmpty(Convert.ToString(formData["hoten"]))) { hoten = Convert.ToString(formData["hoten"]); }
            string taikhoan = "";
            if (formData.Keys.Contains("taikhoan") && !string.IsNullOrEmpty(Convert.ToString(formData["taikhoan"]))) { taikhoan = Convert.ToString(formData["taikhoan"]); }
            long total = 0;
            var data = _userBusiness.Search(page, pageSize, out total, hoten, taikhoan);
            response.TotalItems = total;
            response.Data = data;
            response.Page = page;
            response.PageSize = pageSize;
            return response;
        }
    }
}
