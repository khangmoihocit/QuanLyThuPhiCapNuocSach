using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyThuPhiCapNuocSach
{
    public partial class frmQuenMatKhau: Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["QuanLyThuPhiCapNuocSach"].ConnectionString;

        public frmQuenMatKhau()
        {
            InitializeComponent();
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

        private void btnLayLaiMatKhau_Click(object sender, EventArgs e)
        {
            if (txtEmailDK.Text.Trim() == "") MessageBox.Show("Vui lòng nhập email đăng ký");
            else
            {
                DataTable dataTable = getTaiKhoanFilter($"select * from tblTaiKhoan where sEmail = N'{txtEmailDK.Text}'");
                if (dataTable.Rows.Count > 0)
                {
                    lblThongBao.ForeColor = Color.Blue;
                    lblThongBao.Text = "Tài khoản: " + dataTable.Rows[0]["sTenTaiKhoan"].ToString() + "\n";
                    lblThongBao.Text += "Mật khẩu: " + dataTable.Rows[0]["sEmail"].ToString();
                }
                else
                {
                    lblThongBao.ForeColor = Color.Red;
                    lblThongBao.Text = "Email này chưa được đăng ký";
                }
            }
        }
    }
}
