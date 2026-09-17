using System;

namespace Model
{
    public class LichSuThaoTacModel
    {
        public int id { get; set; }
        public int? nguoidungid { get; set; }
        public string hanhdong { get; set; } // 'TAO', 'SUA', 'XOA'
        public string tenbang { get; set; } // Vd: 'hopdong'
        public int? idbanghi { get; set; }
        public string dulieucu { get; set; } // JSON
        public string dulieumoi { get; set; } // JSON
        public DateTime ngaytao { get; set; }
    }
}
