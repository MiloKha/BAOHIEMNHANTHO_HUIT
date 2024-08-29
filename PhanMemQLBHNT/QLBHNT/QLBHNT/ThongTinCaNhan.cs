using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBHNT
{
    public partial class ThongTinCaNhan : Form
    {
        private MongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<BsonDocument> collection;

        public ThongTinCaNhan()
        {
            InitializeComponent();
            IMongoCollection<BsonDocument> dbCollection = GetMongoDBCollection();
        }

        private IMongoCollection<BsonDocument> GetMongoDBCollection()
        {
            string connectionString = "mongodb://localhost:27017";


            // Tạo MongoClient và kết nối tới cơ sở dữ liệu
            client = new MongoClient(connectionString);
            database = client.GetDatabase("Account");
            collection = database.GetCollection<BsonDocument>("account");
            return collection;

        }
        private void btn_quaylai_Click(object sender, EventArgs e)
        {
            QLChuongTrinh f = new QLChuongTrinh();
            f.Show();
            this.Hide();
        }

        private void btn_capnhat_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ TextBox
            string tenNguoiDungCanSua = textBox1.Text;
            string tenNguoiDungMoi = textBox2.Text;
            string MKMoi = textBox3.Text;

            // Thực hiện cập nhật thông tin người dùng vào cơ sở dữ liệu (ví dụ: cập nhật dữ liệu trong MongoDB)
            var filter = Builders<BsonDocument>.Filter.Eq("TenNguoiDung", tenNguoiDungCanSua);
            var update = Builders<BsonDocument>.Update
                .Set("TenHiểnThi", tenNguoiDungMoi)
                .Set("Mật Khẩu Mới", MKMoi);

            var result = collection.UpdateOne(filter, update);

            // Kiểm tra xem cập nhật có thành công không
            if (result.ModifiedCount > 0)
            {
                // Cập nhật thành công
                MessageBox.Show("Cập nhật người dùng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Không tìm thấy người dùng hoặc có lỗi khi cập nhật
                MessageBox.Show("Không thể cập nhật người dùng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {

            DialogResult dialog;
            dialog = MessageBox.Show("Bạn có muốn thoát hay không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
                Application.Exit();
        }
    }
}
