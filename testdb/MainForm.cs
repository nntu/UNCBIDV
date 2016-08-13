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
                MessageBox.Show(@"Xin Nhập Số.");
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

        private inUNC _unc;

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private Config _cf;

        private void Form1_Load(object sender, EventArgs e)
        {
            _cf = Config.Load();
            tb_chinhanh.Text = _cf.TenCn;
            tb_tentktrichno.Text = _cf.TenTktn;
            tb_tktrichno.Text = _cf.SoTktn;
            tb_ketoantruong.Text = _cf.KeToanTruong;
            tb_ChuTK.Text = _cf.ChuTaiKhoan;
            dtp_denngay.Value = dtp_tungay.Value = DateTime.Now;
        }

        private void bt_capnhattktricno_Click(object sender, EventArgs e)
        {
            _cf.TenCn = tb_chinhanh.Text.Trim();
            _cf.TenTktn = tb_tentktrichno.Text.Trim();
            _cf.SoTktn = tb_tktrichno.Text.Trim();
            _cf.KeToanTruong = tb_ketoantruong.Text.Trim();
            _cf.ChuTaiKhoan = tb_ChuTK.Text.Trim();
            _cf.Save();
        }

        private void bt_in_Click(object sender, EventArgs e)
        {
            _unc = new inUNC();
            if (tb_sotienbs.Text.Length < 2)
            {
                MessageBox.Show(@"Chưa Nhập số tiền");
            }
            else
            {
                _unc.Chinhanh = tb_chinhanh.Text;
                _unc.Tktrichno = tb_tktrichno.Text;
                _unc.Tentktrichno = tb_tentktrichno.Text;
                _unc.sotk = tb_sotk.Text;
                _unc.sotien = Convert.ToDecimal(tb_sotienbs.Text);
                _unc.nguoihuong = tb_nguoihuong.Text;
                if (tb_cmnd.Text == "")
                {
                    _unc.socmnd = "";
                    _unc.noicap = "";
                    _unc.ngaycap = null;
                }
                else
                {
                    _unc.socmnd = tb_cmnd.Text;
                    _unc.noicap = tb_noicap.Text;
                    _unc.ngaycap = dt_ngaycap.Value;
                }
                _unc.nganhangnhhan = tb_nganhang.Text;
                _unc.noidung = tb_noidung.Text;
                _unc.KeToanTruong = tb_ketoantruong.Text;
                _unc.ChuTaiKhoan = tb_ChuTK.Text;
                _unc.phingoai = cb_phingoai.Checked;
                _unc.phitrong = cb_phitrong.Checked;

                var sotienbc = Lib.DocSoThanhChu(tb_sotienbs.Text) + @" đồng";

                var a2 = Lib.ChiaDong(sotienbc, 5);

                _unc.sotienbc1 = a2[0];
                _unc.sotienbc2 = a2[1];

                var a = new viewUNC(_unc);
                if (a.ShowDialog() == DialogResult.Cancel)
                {
                    var re = MessageBox.Show(@"Bạn muốn lưu lại thông tin in unc không", @"Lưu", MessageBoxButtons.OKCancel);
                    if (re == DialogResult.OK)
                    {
                        using (var db = new LiteDatabase(_cf.DatabaseName))
                        {
                            var uncdb = db.GetCollection<UNC>("UNC");
                            UNC ee = new UNC();
                            _unc.CopyTo(ee);
                            ee.sotienbc = sotienbc;
                            ee.NgayIn = DateTime.Now;
                            uncdb.Insert(ee);
                        }
                    }
                };
            }
        }

        private BindingSource _bi;
        private IEnumerable<UNC> _results;

        private void bt_laysol_Click(object sender, EventArgs e)
        {
            using (var db = new LiteDatabase(_cf.DatabaseName))
            {
                var col = db.GetCollection<UNC>("UNC");
                _results = col.Find(c => c.NgayIn >= dtp_tungay.Value && c.NgayIn <= dtp_denngay.Value);
                _bi = new BindingSource();
                _bi.DataSource = _results;
                dataGridView1.DataSource = _bi;
            }
        }

        private void OpenExplorer(string dir)
        {
            var result = MessageBox.Show($"Xuất Bc thành công \n File Lưu tại {dir} \n Bạn Có muốn mở file", @"OpenFile", MessageBoxButtons.YesNo,
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