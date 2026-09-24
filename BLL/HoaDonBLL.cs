using System;
using System.Collections.Generic;
using BLL.Interfaces;
using DAL.Interfaces;
using Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BLL
{
    public class HoaDonBLL : IHoaDonBLL
    {
        private readonly IHoaDonRepository _res;
        private readonly ICuaHangRepository _cuaHangRes;

        public HoaDonBLL(IHoaDonRepository res, ICuaHangRepository cuaHangRes)
        {
            _res = res;
            _cuaHangRes = cuaHangRes;

            QuestPDF.Settings.License = LicenseType.Community;
        }

        public List<HoaDonModel> GetAll()
        {
            return _res.GetAll();
        }

        public HoaDonModel GetById(int id)
        {
            return _res.GetById(id);
        }

        public bool Create(HoaDonModel model)
        {
            return _res.Create(model);
        }

        public bool Update(HoaDonModel model)
        {
            return _res.Update(model);
        }

        public bool Delete(int id)
        {
            return _res.Delete(id);
        }

        public byte[] ExportHoaDonToPdf(int hoadonId)
        {
            var hoadon = _res.GetById(hoadonId);
            if (hoadon == null) return null;

            var cuahang = _cuaHangRes.GetById(hoadon.cuahangid);
            string tenCuaHang = cuahang != null ? cuahang.tencuahang : "Cửa hàng ID " + hoadon.cuahangid;
            string diaChi = cuahang != null ? (cuahang.diachi ?? "N/A") : "N/A";

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("HỆ THỐNG CỬA HÀNG NHƯỢNG QUYỀN FRANCHISE").FontSize(14).Bold().FontColor(Colors.Blue.Darken2);
                        col.Item().Text("HÓA ĐƠN THU PHÍ NHƯỢNG QUYỀN").FontSize(20).Bold().FontColor(Colors.Red.Medium).AlignCenter();
                        col.Item().Text($"Mã hóa đơn: HD-{hoadon.id:D6} | Ngày lập: {DateTime.Now:dd/MM/yyyy}").FontSize(10).Italic().AlignCenter();
                        col.Item().PaddingBottom(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().Text($"Đơn vị nhượng quyền: {tenCuaHang}").Bold();
                        col.Item().Text($"Địa chỉ: {diaChi}");
                        col.Item().Text($"Kỳ thanh toán: {hoadon.kythanhtoan}");
                        col.Item().Text($"Trạng thái: {(hoadon.trangthai == "dathanhtoan" ? "Đã thanh toán" : "Chưa thanh toán")}").FontColor(hoadon.trangthai == "dathanhtoan" ? Colors.Green.Medium : Colors.Red.Medium);

                        col.Item().PaddingVertical(15).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("STT").Bold();
                                header.Cell().Element(CellStyle).Text("Khoản Thu").Bold();
                                header.Cell().Element(CellStyle).AlignRight().Text("Số Tiền (VNĐ)").Bold();

                                static IContainer CellStyle(IContainer c) => c.Background(Colors.Grey.Lighten2).Padding(5).AlignCenter();
                            });

                            table.Cell().Element(CellStyleContent).Text("1");
                            table.Cell().Element(CellStyleContent).Text("Phí nhượng quyền tháng");
                            table.Cell().Element(CellStyleContent).AlignRight().Text($"{hoadon.tienphinhuongquyen:N0}");

                            table.Cell().Element(CellStyleContent).Text("2");
                            table.Cell().Element(CellStyleContent).Text("Thuế VAT");
                            table.Cell().Element(CellStyleContent).AlignRight().Text($"{hoadon.tienthue:N0}");

                            table.Cell().ColumnSpan(2).Element(CellStyleContent).AlignRight().Text("TỔNG TIỀN PHẢI THANH TOÁN:").Bold();
                            table.Cell().Element(CellStyleContent).AlignRight().Text($"{hoadon.tongtien:N0} VNĐ").Bold().FontColor(Colors.Red.Medium);

                            static IContainer CellStyleContent(IContainer c) => c.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5);
                        });
                    });

                    page.Footer().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("ĐẠI DIỆN CỬA HÀNG").Bold().AlignCenter();
                                c.Item().Text("(Ký và ghi rõ họ tên)").Italic().FontSize(10).AlignCenter();
                            });
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("ĐẠI DIỆN THƯƠNG HIỆU").Bold().AlignCenter();
                                c.Item().Text("(Ký và ghi rõ họ tên)").Italic().FontSize(10).AlignCenter();
                            });
                        });
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
