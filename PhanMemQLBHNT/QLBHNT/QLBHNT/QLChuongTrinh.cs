using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBHNT
{
    public partial class QLChuongTrinh : Form
    {
        private MongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<BsonDocument> collection;
        public bool isthoat = true;
        public QLChuongTrinh()
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
        public EventHandler DangXuat;
        private void QLChuongTrinh_Load(object sender, EventArgs e)
        {

        }

        private void btn_dangxuat_Click(object sender, EventArgs e)
        {
            DangXuat(this, new EventArgs());
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            DialogResult dialog;
            dialog = MessageBox.Show("Bạn có muốn thoát hay không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
                Application.Exit();
        }

        private void btnt_them_Click(object sender, EventArgs e)
        {
            {
                // Tại đây, bạn có thể lấy thông tin tên người dùng và chức vụ từ đối tượng taiKhoan
                string tenNguoiDung = textBox3.Text;
                string chucVu = textBox4.Text;

                // Thực hiện thêm tài khoản vào cơ sở dữ liệu hoặc tác vụ thêm mới
                // Ví dụ: Thêm dữ liệu vào MongoDB
                var document = new BsonDocument
                {
                    { "TenNguoiDung", tenNguoiDung },
                    { "ChucVu", chucVu }
                };
                collection.InsertOne(document);
                textBox3.Text = "";
                textBox4.Text = "";

                // Thông báo thành công
                MessageBox.Show("Thêm người dùng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);


            };
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ TextBox
            string tenNguoiDungCanXoa = textBox3.Text;

            // Thực hiện xóa người dùng khỏi cơ sở dữ liệu (ví dụ: xóa dữ liệu trong MongoDB)
            var filter = Builders<BsonDocument>.Filter.Eq("TenNguoiDung", tenNguoiDungCanXoa);
            var result = collection.DeleteOne(filter);

            // Kiểm tra xem xóa có thành công không
            if (result.DeletedCount > 0)
            {
                // Xóa thành công
                MessageBox.Show("Xóa người dùng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Không tìm thấy người dùng hoặc có lỗi khi xóa
                MessageBox.Show("Không thể xóa người dùng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ TextBox
            //string tenNguoiDungCanSua = textBoxTenNguoiDungCanSua.Text;
            string tenNguoiDungMoi = textBox3.Text;
            string chucVuMoi = textBox4.Text;

            // Thực hiện cập nhật thông tin người dùng vào cơ sở dữ liệu (ví dụ: cập nhật dữ liệu trong MongoDB)
            var filter = Builders<BsonDocument>.Filter.Eq("TenNguoiDung", tenNguoiDungMoi);
            var update = Builders<BsonDocument>.Update
                .Set("TenNguoiDung", tenNguoiDungMoi)
                .Set("ChucVu", chucVuMoi);

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

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThongTinCaNhan fr1 = new ThongTinCaNhan();
            fr1.TopLevel = false;
            fr1.AutoScroll = true;

            fr1.Dock = DockStyle.Fill;
            fr1.Show();
        }
    }
}
