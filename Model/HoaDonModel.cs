using System;

namespace Model
{
    public class HoaDonModel
    {
        public int id { get; set; }
        public int cuahangid { get; set; }
        public int hopdongid { get; set; }
        public string kythanhtoan { get; set; } // Vd: '2026-09'
        public decimal tienphinhuongquyen { get; set; }
        public decimal tienthue { get; set; }
        public decimal tongtien { get; set; }
        public string trangthai { get; set; } // 'chuathanhtoan', 'dathanhtoan', 'quahan'
        public string duongdanpdf { get; set; }
        public DateTime ngaytao { get; set; }
        public DateTime? ngayxoa { get; set; }
    }
}
