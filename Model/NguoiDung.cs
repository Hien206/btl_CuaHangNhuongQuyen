using System;

namespace Model
{
    public class NguoiDungModel
    {
        public int id { get; set; }
        public string email { get; set; }
        public string matkhauhash { get; set; }
        public string hoten { get; set; }
        public int vaitroid { get; set; }
        public bool trang_thai_kich_hoat { get; set; }
        public string token_lam_moi { get; set; }
        public DateTime? han_token_lam_moi { get; set; }
        public string token_dat_lai_mat_khau { get; set; }
        public DateTime? han_token_dat_lai { get; set; }
        public DateTime ngay_tao { get; set; }
        public DateTime? ngay_cap_nhat { get; set; }
        public DateTime? ngay_xoa { get; set; }
    }
}
