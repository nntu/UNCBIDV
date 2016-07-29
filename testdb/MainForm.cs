using LiteDB;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
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
            dtp_denngay.Value = dtp_tungay.Value = DateTime.Now;
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

        private void bt_in_Click(object sender, EventArgs e)
        {
            unc = new inUNC();
            if (tb_sotienbs.Text.Length < 2)
            {
                MessageBox.Show("Chưa Nhập số tiền");
            }
            else
            {
                unc.Chinhanh = tb_chinhanh.Text;
                unc.Tktrichno = tb_tktrichno.Text;
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

                var sotienbc = Lib.DocSoThanhChu(tb_sotienbs.Text) + @" đồng";

                var a2 = Lib.ChiaDong(sotienbc, 5);

                unc.sotienbc1 = a2[0];
                unc.sotienbc2 = a2[1];

                viewUNC a = new viewUNC(unc);
                if (a.ShowDialog() == DialogResult.Cancel)
                {
                    var re = MessageBox.Show("Bạn muốn lưu lại thông tin in unc không", "Lưu", MessageBoxButtons.OKCancel);
                    if (re == DialogResult.OK)
                    {
                        using (var db = new LiteDatabase(cf.DatabaseName))
                        {
                            var uncdb = db.GetCollection<UNC>("UNC");
                            UNC ee = new UNC();
                            Lib.CopyTo(unc, ee);
                            ee.sotienbc = sotienbc;
                            ee.NgayIn = DateTime.Now;
                            uncdb.Insert(ee);
                        }
                    }
                };
            }
        }

        private BindingSource bi;
        private IEnumerable<UNC> results;

        private void bt_laysol_Click(object sender, EventArgs e)
        {
            using (var db = new LiteDatabase(cf.DatabaseName))
            {
                var col = db.GetCollection<UNC>("UNC");
                results = col.Find(c => c.NgayIn >= dtp_tungay.Value && c.NgayIn <= dtp_denngay.Value);
                bi = new BindingSource();
                bi.DataSource = results;
                dataGridView1.DataSource = bi;
            }
        }

        private void OpenExplorer(string dir)
        {
            var result = MessageBox.Show($"Xuất Bc thành công \n File Lưu tại {dir} \n Bạn Có muốn mở file", "OpenFile", MessageBoxButtons.YesNo,
                                   MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = dir,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (SaveFileExcel.ShowDialog() == DialogResult.OK)
            {
                using (var fileData = new FileStream(SaveFileExcel.FileName, FileMode.Create))
                {
                    IWorkbook workbook = new HSSFWorkbook();
                    ISheet sheet1 = workbook.CreateSheet("Sheet1");
                    var row = sheet1.CreateRow(0);
                    for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                    {
                        row.CreateCell(i - 1).SetCellValue(dataGridView1.Columns[i - 1].HeaderText);
                    }

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        row = sheet1.CreateRow(i + 1);
                        for (int j = 1; j < dataGridView1.Columns.Count + 1; j++)
                        {
                            if (dataGridView1.Rows[i].Cells[j - 1].Value != null)
                            {
                                row.CreateCell(j - 1).SetCellValue(dataGridView1.Rows[i].Cells[j - 1].Value.ToString());
                            }
                        }
                    }

                    workbook.Write(fileData);
                }

                OpenExplorer(SaveFileExcel.FileName);
            }
        }
    }
}