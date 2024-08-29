using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLBHNT
{
    public partial class frmHoSoBoiThuong : Form
    {
        private IMongoCollection<BsonDocument> collection;
        private MongoClient client;
        private IMongoDatabase database;
        private DataTable dataTable;
        public frmHoSoBoiThuong()
        {
            InitializeComponent();
            string connectionString = "mongodb://localhost:27017"; // Thay đổi tên địa chỉ và cổng MongoDB tại đây
            client = new MongoClient(connectionString);
            database = client.GetDatabase("QLBH");
            collection = database.GetCollection<BsonDocument>("HopDong"); // Thay tên collection mong muốn
            txtmahs.Enabled = false;

        }

        private void frmHoSoBoiThuong_Load(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Mã hợp đồng", typeof(string));
            dataTable.Columns.Add("Mã hồ sơ", typeof(string));
            dataTable.Columns.Add("Ngày yêu cầu", typeof(DateTime));
            dataTable.Columns.Add("Mô tả", typeof(string));
            dataTable.Columns.Add("Tên khách hàng", typeof(string));
            dataTable.Columns.Add("Tên người thụ hưởng", typeof(string));
            dataTable.Columns.Add("Quan hệ", typeof(string));
            dataTable.Columns.Add("Ngày sinh", typeof(DateTime));
            dataTable.Columns.Add("Tình trạng", typeof(string));


            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            comboBox1.Items.Add("50,000,000");
            comboBox1.Items.Add("100,000,000");
            comboBox1.Items.Add("500,000,000");
            this.cbo_tinhtrang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_tinhtrang.FormattingEnabled = true;
            cbo_tinhtrang.Items.Add("True");
            cbo_tinhtrang.Items.Add("False");

            // Lấy dữ liệu từ collection
            var filter = Builders<BsonDocument>.Filter.Empty;
            var documents = collection.Find(filter).ToList();

            // Duyệt qua tất cả các tài liệu và thêm vào DataTable
            foreach (var document in documents)
            {
                // Lấy dữ liệu từ collection Hồ sơ bồi thường
                var hosoboithuong = document["Hosoboithuong"].AsBsonDocument;
                var khachhang = document["Khachhang"].AsBsonDocument;
                var mahs = hosoboithuong["Mahs"].ToString();
                var ngayyeucauValue = hosoboithuong["Ngayyeucau"];

                DateTime? ngayyeucau = null;

                if (ngayyeucauValue != BsonNull.Value && ngayyeucauValue.IsBsonDateTime)
                {
                    ngayyeucau = ngayyeucauValue.AsDateTime;
                    if (ngayyeucau.Value.Kind == DateTimeKind.Utc)
                    {
                        ngayyeucau = ngayyeucau.Value.ToLocalTime();
                    }
                }
                string mota = string.Empty;

                if (hosoboithuong.Contains("Motahoso") && !hosoboithuong["Motahoso"].IsBsonNull)
                {
                    mota = hosoboithuong["Motahoso"].AsString;
                }
                bool tinhtrang = false;

                if (hosoboithuong.Contains("Tinhtrang") && hosoboithuong["Tinhtrang"].IsBoolean)
                {
                    var tinhtrangValue = hosoboithuong["Tinhtrang"];

                    if (tinhtrangValue.IsBoolean)
                    {
                        tinhtrang = tinhtrangValue.AsBoolean;
                    }
                    else if (tinhtrangValue.IsString)
                    {
                        if (bool.TryParse(tinhtrangValue.AsString, out bool parsedValue))
                        {
                            tinhtrang = parsedValue;
                        }
                    }
                }
                var tinhtrangAsString = tinhtrang ? "true" : "false";
                string maHD = document.GetValue("_id").AsObjectId.ToString();
                // Lấy dữ liệu từ collection Người thụ hưởng
                var nguoithuhuong = document["Nguoithuhuong"].AsBsonDocument;
                var tennguoithuhuong = nguoithuhuong["Hoten"].ToString();
                var quanhe = nguoithuhuong["QuanheKH"].AsString;
                var ngaysinh = nguoithuhuong["Ngaysinh"].ToUniversalTime();
                var tenkhachhang = khachhang["Tenkh"].AsString;

                // Thêm dòng vào DataTable
                dataTable.Rows.Add(maHD, mahs, ngayyeucau, mota, tenkhachhang, tennguoithuhuong, quanhe, ngaysinh, tinhtrangAsString);
            }

            // Hiển thị DataTable trên DataGridView
            dgv_HoSoBoiThuong.DataSource = dataTable;
        }
        private void ClearTextBoxes()
        {
            txt_mota.Text = "";
            cbo_tinhtrang.SelectedItem = null;
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {

            if (dgv_HoSoBoiThuong.SelectedCells.Count > 0)
            {
                int selectedRowIndex = dgv_HoSoBoiThuong.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dgv_HoSoBoiThuong.Rows[selectedRowIndex];
                string mahd = txtmahs.Text.Trim();
                string mota = txt_mota.Text.Trim();
                string tinhtrang = cbo_tinhtrang.SelectedItem.ToString();
                DateTime ngayyeucau = dateTimePicker1.Value.ToUniversalTime();

                var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(mahd));
                var document = collection.Find(filter).FirstOrDefault();
                if (document != null)
                {
                    var update = Builders<BsonDocument>.Update
                        .Set("Hosoboithuong.Ngayyeucau", ngayyeucau)
                        .Set("Hosoboithuong.Motahoso", mota)
                        .Set("Hosoboithuong.Tinhtrang", tinhtrang);

                    collection.UpdateOne(filter, update);

                    MessageBox.Show("Cập nhật thành công", "Thông báo");

                    selectedRow.Cells["Ngày yêu cầu"].Value = dateTimePicker1.Value;
                    selectedRow.Cells["Mô tả"].Value = txt_mota.Text;
                    selectedRow.Cells["Tình trạng"].Value = bool.Parse(cbo_tinhtrang.SelectedItem?.ToString() ?? "False");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tài liệu với id cụ thể", "Thông báo");
                }

            }

            ClearTextBoxes();

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Mã hợp đồng", typeof(string));
            dataTable.Columns.Add("Mã hồ sơ", typeof(string));
            dataTable.Columns.Add("Ngày yêu cầu", typeof(DateTime));
            dataTable.Columns.Add("Mô tả", typeof(string));
            dataTable.Columns.Add("Tên khách hàng", typeof(string));
            dataTable.Columns.Add("Tên người thụ hưởng", typeof(string));
            dataTable.Columns.Add("Quan hệ", typeof(string));
            dataTable.Columns.Add("Ngày sinh", typeof(DateTime));
            dataTable.Columns.Add("Tình trạng", typeof(string));


            this.cbo_tinhtrang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_tinhtrang.FormattingEnabled = true;
            cbo_tinhtrang.Items.Add("True");
            cbo_tinhtrang.Items.Add("False");

            // Lấy dữ liệu từ collection
            var filter = Builders<BsonDocument>.Filter.Empty;
            var documents = collection.Find(filter).ToList();

            // Duyệt qua tất cả các tài liệu và thêm vào DataTable
            foreach (var document in documents)
            {
                // Lấy dữ liệu từ collection Hồ sơ bồi thường
                var hosoboithuong = document["Hosoboithuong"].AsBsonDocument;
                var khachhang = document["Khachhang"].AsBsonDocument;
                var mahs = hosoboithuong["Mahs"].ToString();
                var ngayyeucauValue = hosoboithuong["Ngayyeucau"];
                DateTime? ngayyeucau = null;

                if (ngayyeucauValue != BsonNull.Value && ngayyeucauValue.IsBsonDateTime)
                {
                    ngayyeucau = ngayyeucauValue.AsDateTime;
                    if (ngayyeucau.Value.Kind == DateTimeKind.Utc)
                    {
                        ngayyeucau = ngayyeucau.Value.ToLocalTime();
                    }
                }
                string mota = string.Empty;

                if (hosoboithuong.Contains("Motahoso") && !hosoboithuong["Motahoso"].IsBsonNull)
                {
                    mota = hosoboithuong["Motahoso"].AsString;
                }
                bool tinhtrang = false;

                if (hosoboithuong.Contains("Tinhtrang") && hosoboithuong["Tinhtrang"].IsBoolean)
                {
                    var tinhtrangValue = hosoboithuong["Tinhtrang"];

                    if (tinhtrangValue.IsBoolean)
                    {
                        tinhtrang = tinhtrangValue.AsBoolean;
                    }
                    else if (tinhtrangValue.IsString)
                    {
                        if (bool.TryParse(tinhtrangValue.AsString, out bool parsedValue))
                        {
                            tinhtrang = parsedValue;
                        }
                    }
                }
                var tinhtrangAsString = tinhtrang ? "true" : "false";
                string maHD = document.GetValue("_id").AsObjectId.ToString();
                // Lấy dữ liệu từ collection Người thụ hưởng
                var nguoithuhuong = document["Nguoithuhuong"].AsBsonDocument;
                var tennguoithuhuong = nguoithuhuong["Hoten"].ToString();
                var quanhe = nguoithuhuong["QuanheKH"].AsString;
                var ngaysinh = nguoithuhuong["Ngaysinh"].ToUniversalTime();
                var tenkhachhang = khachhang["Tenkh"].AsString;


                // Thêm dòng vào DataTable
                dataTable.Rows.Add(maHD, mahs, ngayyeucau, mota, tenkhachhang, tennguoithuhuong, quanhe, ngaysinh, tinhtrangAsString);
            }

            // Hiển thị DataTable trên DataGridView
            dgv_HoSoBoiThuong.DataSource = dataTable;
        }
        private void dgv_HoSoBoiThuong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem người dùng đã nhấp chuột vào hàng hay không
            if (e.RowIndex >= 0)
            {
                // Lấy giá trị Ngayyeucau từ cell tương ứng trong DataGridView
                object ngayYeuCauValue = dgv_HoSoBoiThuong.Rows[e.RowIndex].Cells["Ngày yêu cầu"].Value;
                DateTime ngayYeuCau;

                // Kiểm tra xem giá trị Ngayyeucau có tồn tại không
                if (ngayYeuCauValue != null && ngayYeuCauValue is DateTime)
                {
                    ngayYeuCau = (DateTime)ngayYeuCauValue;
                }
                else
                {
                    ngayYeuCau = DateTime.Now; // Nếu không có giá trị, sử dụng thời gian hiện tại
                }

                // Hiển thị giá trị Ngayyeucau trên DateTimePicker
                dateTimePicker1.Value = ngayYeuCau;

                // Lấy giá trị Mota từ cell tương ứng trong DataGridView
                string moTa = (string)dgv_HoSoBoiThuong.Rows[e.RowIndex].Cells["Mô tả"].Value;
                string mahs = (string)dgv_HoSoBoiThuong.Rows[e.RowIndex].Cells["Mã hợp đồng"].Value;
                // Hiển thị giá trị Mota trên TextBox
                txt_mota.Text = moTa;
                txtmahs.Text = mahs;
                // Lấy giá trị Tinhtrang từ cell tương ứng trong DataGridView
                string tinhTrang = (string)dgv_HoSoBoiThuong.Rows[e.RowIndex].Cells["Tình trạng"].Value;

                // Hiển thị giá trị Tinhtrang trên TextBox
                cbo_tinhtrang.Text = tinhTrang;
            }
        }

        private void dgv_HoSoBoiThuong_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    } 
}
