using System;

namespace Model
{
    public class ThongBaoModel
    {
        public int id { get; set; }
        public int nguoidungid { get; set; }
        public string loaithongbao { get; set; } // 'CHAM_BAO_CAO', 'DEN_HAN_HOA_DON'
        public string noidung { get; set; }
        public bool dadoc { get; set; }
        public DateTime ngaytao { get; set; }
    }
}
