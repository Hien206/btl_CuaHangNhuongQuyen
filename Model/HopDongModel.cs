using System;

namespace Model
{
    public class HopDongModel
    {
        public int id { get; set; }
        public int cuahangid { get; set; }
        public string sohopdong { get; set; }
        public decimal phantramphi { get; set; }
        public DateTime ngaybatdau { get; set; }
        public DateTime ngayketthuc { get; set; }
        public string trangthai { get; set; } // 'hieuluc', 'hethan', 'chamdut'
        public string duongdanpdf { get; set; }
        public int phienban { get; set; }
        public int? nguoitaoid { get; set; }
        public DateTime ngaytao { get; set; }
        public DateTime? ngaycapnhat { get; set; }
        public DateTime? ngayxoa { get; set; }
    }
}
