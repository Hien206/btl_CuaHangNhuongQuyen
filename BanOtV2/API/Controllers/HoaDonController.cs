using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BLL;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model;
using Asp.Versioning;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HoaDonController : ControllerBase
    {
        private IHoaDonBusiness _hoaDonBusiness;
        private readonly ILogger<HoaDonController> _logger;
        public HoaDonController(IHoaDonBusiness hoaDonBusiness, ILogger<HoaDonController> logger)
        {
            _hoaDonBusiness = hoaDonBusiness;
            _logger = logger;
        }

        [Route("create-hoa-don")]
        [HttpPost]
        public HoaDonModel CreateItem([FromBody] HoaDonModel model)
        {
            _hoaDonBusiness.Create(model);
            return model;
        }

        [Route("update-hoa-don")]
        [HttpPost]
        public HoaDonModel UpdateItem([FromBody] HoaDonModel model)
        {
            _hoaDonBusiness.Update(model);
            return model;
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
            string diachi = "";
            if (formData.Keys.Contains("diachi") && !string.IsNullOrEmpty(Convert.ToString(formData["diachi"]))) { diachi = Convert.ToString(formData["diachi"]); }
            long total = 0;
            var data = _hoaDonBusiness.Search(page, pageSize, out total, hoten, diachi);
            response.TotalItems = total;
            response.Data = data;
            response.Page = page;
            response.PageSize = pageSize;
            return response;
        }

        [Route("get-by-id/{id}")]
        [HttpGet]
        public HoaDonModel GetDatabyID(string id)
        {
            var hoaDon = _hoaDonBusiness.GetDatabyID(id)
                ?? throw new KeyNotFoundException($"Không tìm thấy hóa đơn có mã '{id}'.");
            _logger.LogInformation("Đã đọc hóa đơn {HoaDonId}", id);
            return hoaDon;
        }

        [Route("delete")]
        [HttpPost]
        public IActionResult DeleteUser([FromBody] Dictionary<string, object> formData)
        {
            string ma_hoa_don = "";
            if (formData.Keys.Contains("ma_hoa_don") && !string.IsNullOrEmpty(Convert.ToString(formData["ma_hoa_don"]))) { ma_hoa_don = Convert.ToString(formData["ma_hoa_don"]); }
            _hoaDonBusiness.Delete(ma_hoa_don);
            return Ok();
        }
    }
}
