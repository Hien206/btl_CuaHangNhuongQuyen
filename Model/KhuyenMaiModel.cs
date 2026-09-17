using System;

namespace Model
{
    public class KhuyenMaiModel
    {
        public int id { get; set; }
        public int? cuahangid { get; set; } // null = áp dụng toàn hệ thống
        public string tenkhuyenmai { get; set; }
        public decimal phantramgiam { get; set; }
        public DateTime ngaybatdau { get; set; }
        public DateTime ngayketthuc { get; set; }
        public int? nguoitaoid { get; set; }
        public DateTime ngaytao { get; set; }
        public DateTime? ngayxoa { get; set; }
    }
}
