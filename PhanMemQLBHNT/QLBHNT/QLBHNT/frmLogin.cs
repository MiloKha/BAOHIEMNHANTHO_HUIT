using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;

namespace QLBHNT
{
    public partial class frmLogin : Form
    {
        private MongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<BsonDocument> collection;
        string username = "admin";
        string password = "123";
        public frmLogin()
        {
            InitializeComponent();
            string connectionString = "mongodb://localhost:27017";
            client = new MongoClient(connectionString);
            database = client.GetDatabase("Account");
            collection = database.GetCollection<BsonDocument>("account");
        }
        public frmLogin(IMongoCollection<BsonDocument> account)
        {
            InitializeComponent();
            collection = account;
        }

        private void btn_đangnhap_Click(object sender, EventArgs e)
        {
            if (Kiemtradangnhap(txtusername.Text, txtpassword.Text))
            {
                frmMain f = new frmMain();
                f.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu", "lỗi");
                txtusername.Focus();

            }
            txtusername.Clear();
            txtpassword.Clear();
            txtusername.Focus();
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {

            DialogResult dialog;
            dialog = MessageBox.Show("Bạn có muốn thoát hay không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
                Application.Exit();
        }
        private void F_DangXuat(object sender, EventArgs e)
        {
            (sender as QLChuongTrinh).isthoat = false;
            (sender as QLChuongTrinh).Close();
            this.Show();
        }
        bool Kiemtradangnhap(string username, string password)
        {

            if (username == this.username && password == this.password)
            {
                return true;
            }
            return false;
        }
    }
}
