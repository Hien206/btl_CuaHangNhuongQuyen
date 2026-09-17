using System;
using System.Collections.Generic;

namespace BanMayTinh_NguoiDung.Models
{
    public partial class DanhMuc
    {
        public int MaDanhMuc { get; set; }
        public int? MaDanhMucCha { get; set; }
        public string TenDanhMuc { get; set; } = null!;
        public int? Stt { get; set; }
        public bool TrangThai { get; set; }
    }
}
