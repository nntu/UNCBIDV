using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UyNhiemChiBIDV
{
    public partial class viewUNC : Form
    {
        public viewUNC()
        {
            InitializeComponent();
        }
        inUNC uncl;
        public viewUNC(inUNC unc)
        {
            this.uncl = unc;
            InitializeComponent();
        }


        private void viewUNC_Load(object sender, EventArgs e)
        {
            
         //   UNCA41.SetDataSource(uncl);
            UNCA41.Refresh();
            crv.ReportSource = UNCA41;
            UNCA41.SetParameterValue("TenTKTN", uncl.Tentktrichno);
            UNCA41.SetParameterValue("TKTrichNo", uncl.tktrichno);
            UNCA41.SetParameterValue("ChiNhanh", uncl.Chinhanh);
            UNCA41.SetParameterValue("NguoiHuong", uncl.nguoihuong);
            UNCA41.SetParameterValue("socmnd", uncl.socmnd);
            if (uncl.ngaycap == null)
            {
                UNCA41.SetParameterValue("ngaycap", "");
            }
            else {
                UNCA41.SetParameterValue("ngaycap", String.Format("{0:dd/MM/yyyy}", uncl.ngaycap) );
            }
            UNCA41.SetParameterValue("NoiCap", uncl.noicap);
            UNCA41.SetParameterValue("SoTK", uncl.sotk);

            UNCA41.SetParameterValue("NganHangNhan", uncl.nganhangnhhan);
            UNCA41.SetParameterValue("SoTien", String.Format("{0:0,0} đồng", uncl.sotien));
            UNCA41.SetParameterValue("SoTienbs1", uncl.sotienbc1);
            UNCA41.SetParameterValue("SoTienbs2", uncl.sotienbc2);
            UNCA41.SetParameterValue("NoiDung", uncl.noidung);
            UNCA41.SetParameterValue("KeToanTruong", uncl.KeToanTruong);
            UNCA41.SetParameterValue("ChuTK", uncl.ChuTaiKhoan);
                UNCA41.SetParameterValue("PhiTrong", uncl.phitrong);
            UNCA41.SetParameterValue("PhiNgoai", uncl.phingoai);
        }
    }
}
