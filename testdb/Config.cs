using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UyNhiemChiBIDV
{
   public class Config : AppSettings<Config>
    {
        public string TenCn { get; set  ; } = @"Cần Thơ";
        public string TenTktn { get; set; } = @"";
        public string SoTktn { get; set; } = @"";
        public string KeToanTruong { get; set; } = @"";
        public string ChuTaiKhoan { get; set; } = @"";
        public string DatabaseName { get; set; } = @"data.db";
    }  
}
