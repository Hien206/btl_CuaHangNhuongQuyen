using System;

namespace Model
{
    public class DoanhThuModel
    {
        public int id { get; set; }
        public int cuahangid { get; set; }
        public DateTime ngaybaocao { get; set; }
        public decimal tongdoanhthu { get; set; }
        public string ghichu { get; set; }
        public string trangthai { get; set; } // 'dunghan', 'trehan'
        public int? nguoinhapid { get; set; }
        public DateTime ngaytao { get; set; }
        public DateTime? ngayxoa { get; set; }
    }
}
