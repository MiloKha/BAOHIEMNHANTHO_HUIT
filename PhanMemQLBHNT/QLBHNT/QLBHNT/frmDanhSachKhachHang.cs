using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace QLBHNT
{
    public partial class frmDanhSachKhachHang : Form
    {
        private IMongoCollection<BsonDocument> collection;
        private MongoClient client;
        private IMongoDatabase database;
        private DataTable dataTable;
        public frmDanhSachKhachHang()
        {

            InitializeComponent();
            string connectionString = "mongodb://localhost:27017"; // Thay đổi tên địa chỉ và cổng MongoDB tại đây
            client = new MongoClient(connectionString);
            database = client.GetDatabase("QLBH");
            collection = database.GetCollection<BsonDocument>("HopDong"); // Thay tên collection mong muốn


            dataTable = new DataTable();
            dataTable.Columns.Add("Mã hợp đồng", typeof(string));
            dataTable.Columns.Add("Mã khách hàng", typeof(string));
            dataTable.Columns.Add("Tên khách hàng", typeof(string));
            dataTable.Columns.Add("Địa chỉ", typeof(string));
            dataTable.Columns.Add("Số điện thoại", typeof(string));
            dataTable.Columns.Add("Giới tính", typeof(string));
            dataTable.Columns.Add("Nghề nghiệp", typeof(string));

            // Add dữ liệu combobox giới tính
            this.cbo_gioitinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_gioitinh.FormattingEnabled = true;
            this.cbo_gioitinh.Items.AddRange(new string[] { "Nam", "Nữ" });

            RefreshDataGridView();
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }


        private void RefreshDataGridView()
        {
            // Xóa dữ liệu hiện có trong DataTable
            dataTable.Clear();

            // Lấy dữ liệu từ collection
            var filter = Builders<BsonDocument>.Filter.Empty;
            var documents = collection.Find(filter).ToList();

            // Duyệt qua tất cả các tài liệu và thêm vào DataTable
            foreach (var document in documents)
            {
                // Truy cập vào collection Khachhang
                var maHD = document.GetValue("_id").AsObjectId.ToString();
                var khachhang = document["Khachhang"].AsBsonDocument;

                // Lấy dữ liệu từ collection Khachhang
                var makh = khachhang["Makh"].ToString();
                var tenkh = khachhang["Tenkh"].AsString;
                var diachi = khachhang["Diachi"].AsString;
                var sdt = khachhang["SDT"].AsString;
                var gioitinh = khachhang["Gioitinh"].AsString;
                var nghenghiep = khachhang["Nghenghiep"].AsString;

                // Thêm dòng vàoDataTable
                dataTable.Rows.Add(maHD, makh, tenkh, diachi, sdt, gioitinh, nghenghiep);
            }

            // Hiển thị DataTable trên DataGridView
            dataGridView1.DataSource = dataTable;
        }

        private void txt_ten_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmDanhSachKhachHang_Load(object sender, EventArgs e)
        {
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            txt_ten.KeyPress += txt_ten_KeyPress;
            txt_nghenghiep.KeyPress += txt_ten_KeyPress;
            txt_sdt.KeyPress += txt_sdt_KeyPress;
            txt_sdt.TextChanged += txt_sdt_TextChanged;
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có hàng được chọn trong DataGridView không
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Lấy mã khách hàng từ hàng được chọn
                string maKhachHang = dataGridView1.SelectedRows[0].Cells["Mã khách hàng"].Value.ToString();

                // Lấy thông tin khách hàng từ các textbox
                string tenKhachHang = txt_ten.Text.Trim();
                string diaChi = txt_diachi.Text.Trim();
                string soDienThoai = txt_sdt.Text.Trim();
                string gioiTinh = cbo_gioitinh.SelectedItem?.ToString();
                string ngheNghiep = txt_nghenghiep.Text.Trim();

                // Lấy thông tin khách hàng hiện tại từ MongoDB
                var filter = Builders<BsonDocument>.Filter.Eq("Khachhang.Makh", maKhachHang);
                var document = collection.Find(filter).FirstOrDefault();

                if (document != null)
                {
                    var khachhang = document["Khachhang"].AsBsonDocument;

                    // Kiểm tra và sử dụng lại dữ liệu cũ nếu trường thông tin bị bỏ trống
                    if (string.IsNullOrWhiteSpace(tenKhachHang))
                    {
                        tenKhachHang = khachhang["Tenkh"].AsString;
                    }
                    if (string.IsNullOrWhiteSpace(diaChi))
                    {
                        diaChi = khachhang["Diachi"].AsString;
                    }
                    if (string.IsNullOrWhiteSpace(soDienThoai))
                    {
                        soDienThoai = khachhang["SDT"].AsString;
                    }
                    if (string.IsNullOrWhiteSpace(gioiTinh))
                    {
                        gioiTinh = khachhang["Gioitinh"].AsString;
                    }
                    if (string.IsNullOrWhiteSpace(ngheNghiep))
                    {
                        ngheNghiep = khachhang["Nghenghiep"].AsString;
                    }

                    // Cập nhật thông tin khách hàng trong MongoDB
                    var update = Builders<BsonDocument>.Update
                        .Set("Khachhang.Tenkh", tenKhachHang)
                        .Set("Khachhang.Diachi", diaChi)
                        .Set("Khachhang.SDT", soDienThoai)
                        .Set("Khachhang.Gioitinh", gioiTinh)
                        .Set("Khachhang.Nghenghiep", ngheNghiep);

                    collection.UpdateOne(filter, update);

                    // Hiển thị thông báo thành công
                    MessageBox.Show("Thông tin khách hàng đã được cập nhật thành công.");

                    // Refresh DataGridView
                    RefreshDataGridView();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin khách hàng trong MongoDB.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để sửa thông tin.");
            }
            ClearTextBoxes();
        }
        private void ClearTextBoxes()
        {
            txt_ten.Text = "";
            txt_diachi.Text = "";
            txt_sdt.Text = "";
            cbo_gioitinh.SelectedItem = null;
            txt_nghenghiep.Text = "";
        }
        private void txt_sdt_TextChanged(object sender, EventArgs e)
        {
            string text = txt_sdt.Text;

            // Kiểm tra nếu có ký tự không phải số
            if (text.Any(c => !char.IsDigit(c)))
            {
                // Xóa ký tự không hợp lệ và cập nhật giá trị của textbox
                txt_sdt.Text = new string(text.Where(char.IsDigit).ToArray());
            }

            if (txt_sdt.Text.Length > 10)
            {
                txt_sdt.Text = txt_sdt.Text.Substring(0, 10); // Giới hạn độ dài của số điện thoại là 10 ký tự
            }
        }

        private void txt_ten_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra nếu ký tự không phải là chữ cái hoặc khoảng trắng
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar))
            {
                e.Handled = true; // Ngăn không cho ký tự nhập vào
            }
        }

        private void txt_sdt_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra nếu ký tự không phải là số và không phải là phím Backspace
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Ngăn không cho ký tự nhập vào
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có hàng được chọn không
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                // Lấy dòng được chọn
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Lấy giá trị từ các ô trong dòng
                string tenKhachHang = row.Cells["Tên khách hàng"].Value.ToString();
                string diaChi = row.Cells["Địa chỉ"].Value.ToString();
                string soDienThoai = row.Cells["Số điện thoại"].Value.ToString();
                string gioiTinh = row.Cells["Giới tính"].Value.ToString();
                string ngheNghiep = row.Cells["Nghề nghiệp"].Value.ToString();

                // Hiển thị thông tin lên các TextBox tương ứng
                txt_ten.Text = tenKhachHang;
                txt_diachi.Text = diaChi;
                txt_sdt.Text = soDienThoai;
                cbo_gioitinh.SelectedItem = gioiTinh;
                txt_nghenghiep.Text = ngheNghiep;
            }
        }
    }
}
