using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyThuPhiCapNuocSach
{
    public partial class frmDangKy: Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["QuanLyThuPhiCapNuocSach"].ConnectionString;

        public frmDangKy()
        {
            InitializeComponent();
        }

        //check tên tài khoản và mật khẩu
        private bool checkInputTaiKhoan(string s)
        {
            return Regex.IsMatch(s, "^[a-zA-Z0-9]{6,24}$");
        }

        //check định dạng email
        private bool checkInputEmail(string email)
        {
            //chỉ nhận từ a-z, A-Z, 0-9, _, . , từ 3 đến 20 kí tự, và theo định da @gmail...
            return Regex.IsMatch(email, "^[a-zA-Z0-9_.]{3,20}@gmail.com$");
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

        private void addTaiKhoan()
        {
            string query = "spTaiKhoan_Insert";
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@sTenTaiKhoan", SqlDbType.NVarChar, 50)).Value = txtTenTaiKhoan.Text;
                        command.Parameters.Add(new SqlParameter("@sMatKhau", SqlDbType.NVarChar, 50)).Value = txtMatKhau.Text;
                        command.Parameters.Add(new SqlParameter("@sEmail", SqlDbType.NVarChar, 50)).Value = txtEmail
                            .Text;

                        int n = command.ExecuteNonQuery();
                        if(n > 0)
                        {
                            MessageBox.Show("Tạo tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Tạo tài khoản thất bại");
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            //check input tên tk
            if (!checkInputTaiKhoan(txtTenTaiKhoan.Text))
            {
                MessageBox.Show("tên tài khoản không được chứa kí đặc biệt và từ phải từ 6 - 24 kí tự");
                return;
            }

            //check mật khẩu
            if (!checkInputTaiKhoan(txtMatKhau.Text))
            {
                MessageBox.Show("mật khẩu không được chứa kí đặc biệt và từ phải từ 6 - 24 kí tự");
                return;
            }

            //check email
            if (!checkInputEmail(txtEmail.Text))
            {
                MessageBox.Show("email không p được chứa kí tự đặc biệt và email phải có định dạng '@gmail.com'");
                return;
            }

            //xac nhan mk
            if (txtXacNhanMK.Text != txtMatKhau.Text)
            {
                MessageBox.Show("mật khẩu không khớp");
                return;
            }

            //check tên tài khoản tồn tại chx
            if(getTaiKhoanFilter($"select * from tblTaiKhoan where sTenTaiKhoan = N'{txtTenTaiKhoan.Text}'").Rows.Count > 0)
            {
                MessageBox.Show("tên tài khoản đã tồn tại");
                return;
            }

            //check email ton tai
            if(getTaiKhoanFilter($"select * from tblTaiKhoan where sEmail = N'{txtEmail.Text}'").Rows.Count > 0)
            {
                MessageBox.Show("email này đã được đăng ký");
                return;
            }
            addTaiKhoan();
            this.Close();
        }
    }
}
