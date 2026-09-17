using System;

namespace Model
{
    public class CuaHangModel
    {
        public int id { get; set; }
        public string macuahang { get; set; }
        public string tencuahang { get; set; }
        public int chu_cua_hang_id { get; set; }
        public string diachi { get; set; }
        public string trangthai { get; set; } // 'hoatdong', 'choduyet', 'tamngung'
        public int? nguoi_tao_id { get; set; }
        public DateTime ngay_tao { get; set; }
        public DateTime? ngay_cap_nhat { get; set; }
        public DateTime? ngay_xoa { get; set; }
    }
}
