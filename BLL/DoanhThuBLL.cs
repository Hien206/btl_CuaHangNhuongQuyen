using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using BLL.Interfaces;
using DAL.Interfaces;
using Model;

namespace BLL
{
    public class DoanhThuBLL : IDoanhThuBLL
    {
        private readonly IDoanhThuRepository _res;
        private readonly ICuaHangRepository _cuaHangRes;

        public DoanhThuBLL(IDoanhThuRepository res, ICuaHangRepository cuaHangRes)
        {
            _res = res;
            _cuaHangRes = cuaHangRes;
        }

        public List<DoanhThuModel> GetAll()
        {
            return _res.GetAll();
        }

        public DoanhThuModel GetById(int id)
        {
            return _res.GetById(id);
        }

        public bool Create(DoanhThuModel model)
        {
            return _res.Create(model);
        }

        public bool Update(DoanhThuModel model)
        {
            return _res.Update(model);
        }

        public bool Delete(int id)
        {
            return _res.Delete(id);
        }

        public byte[] ExportDoanhThuToExcel()
        {
            var listDoanhThu = _res.GetAll();
            var listCuaHang = _cuaHangRes.GetAll();
            var mapCuaHang = new Dictionary<int, string>();
            foreach (var ch in listCuaHang)
            {
                mapCuaHang[ch.id] = ch.tencuahang + " (" + ch.macuahang + ")";
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("BaoCaoDoanhThu");

                // Title
                worksheet.Cell(1, 1).Value = "BÁO CÁO DOANH THU CHUỖI NHƯỢNG QUYỀN";
                worksheet.Range(1, 1, 1, 6).Merge();
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Date
                worksheet.Cell(2, 1).Value = "Ngày xuất báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                worksheet.Range(2, 1, 2, 6).Merge();
                worksheet.Cell(2, 1).Style.Font.Italic = true;
                worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Headers
                string[] headers = new string[] { "STT", "Cửa Hàng", "Ngày Báo Cáo", "Tổng Doanh Thu (VNĐ)", "Ghi Chú", "Trạng Thái" };
                for (int col = 0; col < headers.Length; col++)
                {
                    var cell = worksheet.Cell(4, col + 1);
                    cell.Value = headers[col];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1F4E78");
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                // Fill Data
                int row = 5;
                int stt = 1;
                foreach (var dt in listDoanhThu)
                {
                    worksheet.Cell(row, 1).Value = stt++;
                    worksheet.Cell(row, 2).Value = mapCuaHang.ContainsKey(dt.cuahangid) ? mapCuaHang[dt.cuahangid] : "Cửa hàng ID " + dt.cuahangid;
                    worksheet.Cell(row, 3).Value = dt.ngaybaocao.ToString("dd/MM/yyyy");

                    var cellMoney = worksheet.Cell(row, 4);
                    cellMoney.Value = dt.tongdoanhthu;
                    cellMoney.Style.NumberFormat.Format = "#,##0";

                    worksheet.Cell(row, 5).Value = dt.ghichu ?? "";
                    worksheet.Cell(row, 6).Value = dt.trangthai == "dunghan" ? "Đúng hạn" : "Trễ hạn";
                    row++;
                }

                // Auto fit columns
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
