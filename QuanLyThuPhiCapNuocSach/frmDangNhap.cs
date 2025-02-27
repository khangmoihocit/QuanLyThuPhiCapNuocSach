using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyThuPhiCapNuocSach
{
    public partial class frmDangNhap: Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["QuanLyThuPhiCapNuocSach"].ConnectionString;

        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void lkDangKy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDangKy frmDangKy = new frmDangKy();
            frmDangKy.ShowDialog();
        }

        private DataTable getTaiKhoanFilter(string query)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
                sqlConnection.Close();
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (txtTenTaiKhoan.Text.Trim() == "") MessageBox.Show("Vui lòng nhập tên tài khoản");
            else if (txtMatKhau.Text.Trim() == "") MessageBox.Show("Vui lòng nhập mật khẩu");
            else
            {
                string query = $"select * from tblTaiKhoan where sTenTaiKhoan = '{txtTenTaiKhoan.Text}' and sMatKhau = '{txtMatKhau.Text}'";
                if (getTaiKhoanFilter(query).Rows.Count > 0)
                {
                    DialogResult result = MessageBox.Show("Đăng nhập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (result == DialogResult.OK)
                    {
                        frmTrangChu frmTrangChu = new frmTrangChu();
                        frmTrangChu.ShowDialog();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Tên tài khoản hoặc mật khẩu không chính xác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void lkQuenMatKhau_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmQuenMatKhau frmQuenMatKhau = new frmQuenMatKhau();
            frmQuenMatKhau.ShowDialog();
        }
    }
}
