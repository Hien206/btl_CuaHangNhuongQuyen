using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace btl_CuaHangNhuongQuyenUSER.Controllers
{
    [Authorize(Roles = "CHU_CUA_HANG,ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonBLL _hoaDonBLL;

        public HoaDonController(IHoaDonBLL hoaDonBLL)
        {
            _hoaDonBLL = hoaDonBLL;
        }

        // Chủ cửa hàng xem danh sách hóa đơn cần thanh toán
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_hoaDonBLL.GetAll());
        }

        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(int id)
        {
            var res = _hoaDonBLL.GetById(id);
            if (res == null) return NotFound(new { message = "Không tìm thấy hóa đơn" });
            return Ok(res);
        }

        // Chủ cửa hàng tải hóa đơn PDF về lưu trữ kế toán
        [HttpGet("export-pdf/{id}")]
        public IActionResult ExportPdf(int id)
        {
            var pdfBytes = _hoaDonBLL.ExportHoaDonToPdf(id);
            if (pdfBytes == null) return NotFound(new { message = "Không tìm thấy hóa đơn" });
            return File(pdfBytes, "application/pdf", $"HoaDon_NhuongQuyen_{id}.pdf");
        }
    }
}
