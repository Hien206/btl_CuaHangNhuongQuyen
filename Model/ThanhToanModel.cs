using System;

namespace Model
{
    public class ThanhToanModel
    {
        public int id { get; set; }
        public int hoadonid { get; set; }
        public decimal sotien { get; set; }
        public string phuongthuc { get; set; } // 'chuyenkhoan'
        public string magiaodich { get; set; }
        public DateTime ngaythanhtoan { get; set; }
    }
}
