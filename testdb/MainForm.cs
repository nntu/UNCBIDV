using System;
using System.Windows.Forms;

namespace UyNhiemChiBIDV
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }


        private void tb_sotienbs_TextChanged_1(object sender, EventArgs e)
        {
            string sotien = tb_sotienbs.Text;
            if (System.Text.RegularExpressions.Regex.IsMatch(sotien, "[^0-9]"))
            {
                MessageBox.Show("Xin Nhập Số.");
                tb_sotienbs.Text.Remove(tb_sotienbs.Text.Length - 1);
            }
            else
            {
                if (sotien.Length > 1)
                {
                    tb_sotienbc.Text = Lib.DocSoThanhChu(sotien) + @" đồng";
                }
            }
        }

        private inUNC unc;

        private void button1_Click(object sender, EventArgs e)
        {
            unc = new inUNC();
            if (tb_sotienbs.Text.Length < 2)
            {
                MessageBox.Show("Chưa Nhập số tiền");
            }
            else
            {
                unc.Chinhanh = tb_chinhanh.Text;
                unc.tktrichno = tb_tktrichno.Text;
                unc.Tentktrichno = tb_tentktrichno.Text;
                unc.sotk = tb_sotk.Text;
                unc.sotien = Convert.ToDecimal(tb_sotienbs.Text);
                unc.nguoihuong = tb_nguoihuong.Text;
                if (tb_cmnd.Text == "")
                {
                    unc.socmnd = "";
                    unc.noicap = "";
                    unc.ngaycap = null;
                }
                else
                {
                    unc.socmnd = tb_cmnd.Text;
                    unc.noicap = tb_noicap.Text;
                    unc.ngaycap = dt_ngaycap.Value;
                }
                unc.nganhangnhhan = tb_nganhang.Text;
                unc.noidung = tb_noidung.Text;
                unc.KeToanTruong = tb_ketoantruong.Text;
                unc.ChuTaiKhoan = tb_ChuTK.Text;
                unc.phingoai = cb_phingoai.Checked;
                unc.phitrong = cb_phitrong.Checked;

                var sotienbs = Lib.DocSoThanhChu(tb_sotienbs.Text) + @" đồng";

                var sotu = Lib.GetNoOfWords(sotienbs);
                unc.sotienbc1 = sotienbs.CropWholeWords(12);
                unc.sotienbc2 = sotienbs.Replace(sotienbs.CropWholeWords(12), "");
                viewUNC a = new viewUNC(unc);
                a.Show();
            }
        }

        private Config cf;

        private void Form1_Load(object sender, EventArgs e)
        {
            cf = Config.Load();
            tb_chinhanh.Text = cf.TenCN;
            tb_tentktrichno.Text = cf.TenTKTN;
            tb_tktrichno.Text = cf.SoTKTN;
            tb_ketoantruong.Text = cf.KeToanTruong;
            tb_ChuTK.Text = cf.ChuTaiKhoan;
        }

        private void bt_capnhattktricno_Click(object sender, EventArgs e)
        {
            cf.TenCN = tb_chinhanh.Text.Trim();
            cf.TenTKTN = tb_tentktrichno.Text.Trim();
            cf.SoTKTN = tb_tktrichno.Text.Trim();
            cf.KeToanTruong = tb_ketoantruong.Text.Trim();
            cf.ChuTaiKhoan = tb_ChuTK.Text.Trim();
            cf.Save();
        }
    }
}