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
    public partial class frmQLHopDong : Form
    {
        private IMongoCollection<BsonDocument> collection;

        public frmQLHopDong(IMongoCollection<BsonDocument> dbCollection)
        {
            InitializeComponent();
            collection = dbCollection;
        }

        private void frmQLHopDong_Load(object sender, EventArgs e)
        {
            this.cbo_tenBH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_tenBH.FormattingEnabled = true;
            cbo_tenBH.Items.AddRange(new string[]
                {
                    "Bảo hiểm trọn đời",
                    "Bảo hiểm sinh kỳ",
                    "Bảo hiểm tử kỳ",
                    "Bảo hiểm hỗn hợp",
                    "Bảo hiểm trả tiền định kỳ",
                    "Bảo hiểm liên kết đầu tư",
                    "Bảo hiểm hưu trí"
                });

            this.cbo_stBH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_stBH.FormattingEnabled = true;
            cbo_stBH.Items.Add("5,000,000");
            cbo_stBH.Items.Add("10,000,000");

            var filter = Builders<BsonDocument>.Filter.Empty;
            var documents = collection.Find(filter).ToList();

            dgv_HopDong.Columns.Add("maHD", "Mã HĐ");
            dgv_HopDong.Columns.Add("ngayKy", "Ngày bắt đầu");
            dgv_HopDong.Columns.Add("ngayKT", "Ngày kết thúc");
            dgv_HopDong.Columns.Add("loaiBH", "Loại bảo hiểm");
            dgv_HopDong.Columns.Add("giaTri", "Giá trị bảo hiểm");
            dgv_HopDong.Columns.Add("tenKH", "Tên KH");
            dgv_HopDong.Columns.Add("diaChi", "Địa chỉ");
            dgv_HopDong.Columns.Add("sdt", "SĐT");
            dgv_HopDong.Columns.Add("gioiTinh", "Giới tính");
            dgv_HopDong.Columns.Add("ngheNghiep", "Nghề nghiệp");

            foreach (var document in documents)
            {
                // Truy cập các trường dữ liệu trong tài liệu JSON
                var maHD = document.GetValue("_id").AsObjectId.ToString();
                var ngayKy = document.GetValue("Ngaybatdau").ToUniversalTime().ToUniversalTime().ToLocalTime().ToString("dd/MM/yyyy");
                var ngayKT = document.GetValue("Ngayketthuc").ToUniversalTime().ToUniversalTime().ToLocalTime().ToString("dd/MM/yyyy");
                var loaiBH = document["BaoHiem"]["Tenbaohiem"].AsString;
                var giaTri = document["Sotienbaohiem"].IsDouble ? document["Sotienbaohiem"].AsDouble.ToString() : document["Sotienbaohiem"].AsString;
                var tenKH = document["Khachhang"]["Tenkh"].AsString;
                var diaChi = document["Khachhang"]["Diachi"].AsString;
                var sdt = document["Khachhang"]["SDT"].AsString;
                var gioiTinh = document["Khachhang"]["Gioitinh"].AsString;
                var ngheNghiep = document["Khachhang"]["Nghenghiep"].AsString;

                // Thêm thông tin hợp đồng vào dataGridView1
                dgv_HopDong.Rows.Add(maHD, ngayKy, ngayKT, loaiBH, giaTri, tenKH, diaChi, sdt, gioiTinh, ngheNghiep);

                // Lấy hàng đầu tiên trong DataGridView
                if (dgv_HopDong.Rows.Count > 0)
                {
                    DataGridViewRow selectedRow = dgv_HopDong.Rows[0];

                    // Lấy giá trị số tiền bảo hiểm hiện tại từ cơ sở dữ liệu
                    double.TryParse(selectedRow.Cells["giaTri"].Value.ToString(), out double Sotienbaohiem_Hientai);

                    // Gán giá trị mặc định cho cbo_stBH với định dạng số có dấu chấm hoặc dấu phẩy
                    cbo_stBH.Text = Sotienbaohiem_Hientai.ToString("#,##0");
                }

                //Tạo ràng buộc cho các textbox
                txt_timkiem.KeyPress += new KeyPressEventHandler(txt_timkiem_KeyPress);
                dtp_ngayBD.ValueChanged += dtp_ngayBD_ValueChanged;
                dtp_ngayKT.ValueChanged += dtp_ngayKT_ValueChanged;
            }
        }

        private double Sotienbaohiem_Hientai = 0;

        private bool KtraNgayBDNgayKT(string ngayBDText, string ngayKTText, DataGridViewRow selectedRow)
        {
            // Kiểm tra xem hai TextBox có giá trị hay không
            if (string.IsNullOrEmpty(ngayBDText) || string.IsNullOrEmpty(ngayKTText))
            {
                return true; // Trả về true để cho phép lấy lại dữ liệu từ DataGridView
            }

            // Chuyển đổi ngày bắt đầu và ngày kết thúc sang kiểu DateTime
            if (!DateTime.TryParse(ngayBDText, out DateTime ngayBD) || !DateTime.TryParse(ngayKTText, out DateTime ngayKT))
            {
                return false;
            }

            // Kiểm tra xem ngày kết thúc có cách ngày bắt đầu ít nhất 5 năm hay không
            if (ngayKT < ngayBD.AddYears(5))
            {
                return false;
            }
            // Kiểm tra xem ngày kết thúc có lớn hơn ngày hiện tại hay không
            DateTime ngayHienTai = DateTime.Now.Date;
            if (ngayKT < ngayHienTai)
            {
                return false;
            }

            // Kiểm tra xem ngày bắt đầu có nhỏ hơn ngày ký ban đầu hay không
            string ngayKyText = selectedRow.Cells["ngayKy"].Value.ToString();
            DateTime ngayKy = DateTime.Parse(ngayKyText);
            if (ngayBD < ngayKy)
            {
                return false;
            }

            return true;
        }

        // Khai báo một Dictionary để lưu trữ giá trị bảo hiểm cho mỗi hợp đồng
        Dictionary<string, double> sotienBaoHiem_luutru = new Dictionary<string, double>();


        private void btnxoahopdong_Click(object sender, EventArgs e)
        {

        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            //Kích hoạt kiểm tra ngày
             validateDate = true;

            // Lấy giá trị từ DateTimePicker "dtp_ngayBD" và "dtp_ngayKT"
            DateTime ngayBD = dtp_ngayBD.Value;
            DateTime ngayKT = dtp_ngayKT.Value;

            // Lấy giá trị từ ComboBox "cbo_stBH"
            string stBH = cbo_stBH.Text;

            // Kiểm tra xem ngày bắt đầu và ngày kết thúc có hợp lệ hay không
            if (dgv_HopDong.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hàng trong danh sách.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!KtraNgayBDNgayKT(ngayBD.ToString(), ngayKT.ToString(), dgv_HopDong.SelectedRows[0]))
            {
                MessageBox.Show("Ngày kết thúc phải cách ngày bắt đầu ít nhất 5 năm và lớn hơn hoặc bằng ngày ký ban đầu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra xem người dùng đã chọn một hàng trong DataGridView chưa
            if (dgv_HopDong.SelectedRows.Count > 0)
            {
                // Lấy thông tin hợp đồng từ hàng được chọn
                DataGridViewRow selectedRow = dgv_HopDong.SelectedRows[0];
                string maHD = selectedRow.Cells["maHD"].Value.ToString();

                // Lấy dữ liệu từ các ComboBox
                string tenBH = string.IsNullOrEmpty(cbo_tenBH.Text) ? selectedRow.Cells["loaiBH"].Value.ToString() : cbo_tenBH.Text;

                // Kiểm tra xem hợp đồng đã có giá trị bảo hiểm trong Dictionary chưa
                if (sotienBaoHiem_luutru.ContainsKey(maHD))
                {
                    // Lấy giá trị bảo hiểm hiện tại của hợp đồng
                    double sotienBaoHiemHientai = sotienBaoHiem_luutru[maHD];

                    // Lấy giá trị bảo hiểm cũ từ MongoDB
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(maHD));
                    var projection = Builders<BsonDocument>.Projection.Include("Sotienbaohiem");
                    var document = collection.Find(filter).Project(projection).FirstOrDefault();
                    double sotienBaoHiemCu = document["Sotienbaohiem"].AsDouble;

                    // Cộng dồn giá trị bảo hiểm cũ và giá trị bảo hiểm mới
                    double sotienBaoHiemCapNhat = sotienBaoHiemCu + double.Parse(cbo_stBH.Text);

                    // Cập nhật thông tin hợp đồng trong cơ sở dữ liệu
                    var update = Builders<BsonDocument>.Update
                        .Set("BaoHiem.Tenbaohiem", tenBH)
                        .Set("Ngaybatdau", ngayBD)
                        .Set("Ngayketthuc", ngayKT)
                        .Set("Sotienbaohiem", sotienBaoHiemCapNhat);

                    collection.UpdateOne(filter, update);

                    // Cập nhật thông tin hợp đồng trong DataGridView
                    selectedRow.Cells["loaiBH"].Value = tenBH;
                    selectedRow.Cells["ngayKy"].Value = ngayBD.ToString("dd/MM/yyyy");
                    selectedRow.Cells["ngayKT"].Value = ngayKT.ToString("dd/MM/yyyy");
                    selectedRow.Cells["giaTri"].Value = sotienBaoHiemCapNhat.ToString();

                    // Cập nhật giá trị bảo hiểm mới trong Dictionary
                    sotienBaoHiem_luutru[maHD] = sotienBaoHiemCapNhat;

                    // Thông báo thành công
                    MessageBox.Show("Hợp đồng đã được sửa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Thêm giátrị bảo hiểm mới vào Dictionary nếu hợp đồng chưa có giá trị bảo hiểm
                    double sotienBaoHiem = double.Parse(cbo_stBH.Text);

                    // Lấy giá trị bảo hiểm cũ từ MongoDB
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(maHD));
                    var projection = Builders<BsonDocument>.Projection.Include("Sotienbaohiem");
                    var document = collection.Find(filter).Project(projection).FirstOrDefault();
                    var sotienBaoHiemValue = document["Sotienbaohiem"];
                    double sotienBaoHiemCu;

                    if (sotienBaoHiemValue.BsonType == BsonType.Double)
                    {
                        sotienBaoHiemCu = sotienBaoHiemValue.AsDouble;
                    }
                    else if (sotienBaoHiemValue.BsonType == BsonType.String)
                    {
                        if (double.TryParse(sotienBaoHiemValue.AsString, out double parsedValue))
                        {
                            sotienBaoHiemCu = parsedValue;
                        }
                        else
                        {
                            // Xử lý trường hợp giá trị không thể chuyển đổi thành kiểu double
                            // Bạn có thể chọn ném ra một ngoại lệ, gán một giá trị mặc định, hoặc xử lý theo yêu cầu của bạn
                            // Ví dụ:
                            sotienBaoHiemCu = 0.0; // Gán giá trị mặc định là 0.0
                        }
                    }
                    else
                    {
                        sotienBaoHiemCu = 0.0;
                    }

                    // Cộng dồn giá trị bảo hiểm cũ và giá trị bảo hiểm mới
                    double sotienBaoHiemCapNhat = sotienBaoHiemCu + double.Parse(cbo_stBH.Text);

                    // Cập nhật thông tin hợp đồng trong cơ sở dữ liệu
                    var update = Builders<BsonDocument>.Update
                        .Set("BaoHiem.Tenbaohiem", tenBH)
                        .Set("Ngaybatdau", ngayBD)
                        .Set("Ngayketthuc", ngayKT)
                        .Set("Sotienbaohiem", sotienBaoHiemCapNhat);

                    collection.UpdateOne(filter, update);

                    // Cập nhật thông tin hợp đồng trong DataGridView
                    selectedRow.Cells["loaiBH"].Value = tenBH;
                    selectedRow.Cells["ngayKy"].Value = ngayBD.ToString("dd/MM/yyyy");
                    selectedRow.Cells["ngayKT"].Value = ngayKT.ToString("dd/MM/yyyy");
                    selectedRow.Cells["giaTri"].Value = sotienBaoHiemCapNhat.ToString();

                    // Thêm giá trị bảo hiểm mới vào Dictionary
                    sotienBaoHiem_luutru.Add(maHD, sotienBaoHiemCapNhat);

                    // Thông báo thành công
                    MessageBox.Show("Hợp đồng đã được sửa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Hiển thị thông báo nếu không có hợp đồng nào được chọn
                MessageBox.Show("Vui lòng chọn một hợp đồng để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            ClearTextBoxes();

            // Tắt kiểm tra ngày
            validateDate = false;
        }
        private void ClearTextBoxes()
        {
            cbo_tenBH.SelectedItem = null;
            cbo_stBH.SelectedItem = null;
            txt_timkiem.Text = "";
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng đã chọn một hàng trong DataGridView chưa
            if (dgv_HopDong.SelectedRows.Count > 0)
            {
                // Lấy thông tin hợp đồng từ hàng được chọn
                DataGridViewRow selectedRow = dgv_HopDong.SelectedRows[0];
                string maHD = selectedRow.Cells["maHD"].Value.ToString();

                // Xác nhận xóa hợp đồng
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa hợp đồng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Xóa hợp đồng khỏi cơ sở dữ liệu
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(maHD));
                    collection.DeleteOne(filter);

                    // Xóa hợp đồng khỏi DataGridView
                    dgv_HopDong.Rows.Remove(selectedRow);

                    // Thông báo thành công
                    MessageBox.Show("Hợp đồng đã được xóa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Hiển thị thông báo nếu không có hợp đồng nào được chọn
                MessageBox.Show("Vui lòng chọn một hợp đồng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }
        private void RefreshDataGridView()
        {
            // Xóa dữ liệu hiện tại trong DataGridView
            dgv_HopDong.Rows.Clear();

            // Kiểm tra xem collection có được khởi tạo hay không
            if (collection != null)
            {
                // Lấy dữ liệu mới từ cơ sở dữ liệu MongoDB và đưa vào DataGridView
                var hopDongs = collection.Find(new BsonDocument()).ToList();
                foreach (var hopDong in hopDongs)
                {
                    // Kiểm tra các trường có tồn tại hay không trước khi truy cập
                    if (hopDong.Contains("_id") && hopDong["_id"].IsObjectId)
                    {
                        string maHD = hopDong["_id"].AsObjectId.ToString();
                        // Lấy thông tin từ các trường của hợp đồng
                        if (hopDong.Contains("Ngaybatdau") && hopDong["Ngaybatdau"].IsDateTime)
                        {
                            string ngayKy = hopDong["Ngaybatdau"].ToUniversalTime().ToUniversalTime().ToLocalTime().ToString("dd/MM/yyyy");
                            // Lấy thông tin từ các trường khác của hợp đồng
                            if (hopDong.Contains("Ngayketthuc") && hopDong["Ngayketthuc"].IsDateTime)
                            {
                                string ngayKT = hopDong["Ngayketthuc"].ToUniversalTime().ToUniversalTime().ToLocalTime().ToString("dd/MM/yyyy");
                                if (hopDong.Contains("BaoHiem") && hopDong["BaoHiem"].IsBsonDocument && hopDong["BaoHiem"].AsBsonDocument.Contains("Tenbaohiem"))
                                {
                                    string loaiBH = hopDong["BaoHiem"]["Tenbaohiem"].AsString;
                                    // Lấy thông tin từ các trường khác của hợp đồng
                                    if (hopDong.Contains("Sotienbaohiem"))
                                    {
                                        string giaTri = hopDong["Sotienbaohiem"].ToString();
                                        // Lấy thông tin từ các trường khác của hợp đồng
                                        if (hopDong.Contains("Khachhang") && hopDong["Khachhang"].IsBsonDocument && hopDong["Khachhang"].AsBsonDocument.Contains("Tenkh"))
                                        {
                                            string tenKH = hopDong["Khachhang"]["Tenkh"].AsString;
                                            // Lấy thông tin từ các trường khác của hợp đồng
                                            if (hopDong["Khachhang"].AsBsonDocument.Contains("Diachi") && hopDong["Khachhang"].AsBsonDocument.Contains("SDT") && hopDong["Khachhang"].AsBsonDocument.Contains("Gioitinh") && hopDong["Khachhang"].AsBsonDocument.Contains("Nghenghiep"))
                                            {
                                                string diaChi = hopDong["Khachhang"]["Diachi"].AsString;
                                                string sdt = hopDong["Khachhang"]["SDT"].AsString;
                                                string gioiTinh = hopDong["Khachhang"]["Gioitinh"].AsString;
                                                string ngheNghiep = hopDong["Khachhang"]["Nghenghiep"].AsString;

                                                // Thêm hàng mới vào DataGridView
                                                dgv_HopDong.Rows.Add(maHD, ngayKy, ngayKT, loaiBH, giaTri, tenKH, diaChi, sdt, gioiTinh, ngheNghiep);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        
        private void txttimkiem_TextChanged(object sender, EventArgs e)
        {

        }
        private void txt_timkiem_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra nếu phím được nhấn là phím điều khiển
            if (char.IsControl(e.KeyChar))
            {
                // Cho phép các ký tự điều khiển như Backspace
                e.Handled = false;
            }
            // Kiểm tra nếu phím được nhấn là chữ cái
            else if (char.IsLetter(e.KeyChar))
            {
                // Cho phép các chữ cái
                e.Handled = false;
            }
            else
            {
                // Chặn bất kỳ ký tự khác
                e.Handled = true;
            }
        }

        private void dgv_HopDong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy thông tin từ hàng được chọn trong DataGridView
                DataGridViewRow selectedRow = dgv_HopDong.Rows[e.RowIndex];
                string tenBH = selectedRow.Cells["loaiBH"].Value.ToString();
                string ngayBD = selectedRow.Cells["ngayKy"].Value.ToString();
                string ngayKT = selectedRow.Cells["ngayKT"].Value.ToString();
                string stBH = selectedRow.Cells["giaTri"].Value.ToString();

                // Hiển thị thông tin trong các TextBox và ComboBox
                cbo_tenBH.Text = tenBH;
                dtp_ngayBD.Value = DateTime.Parse(ngayBD);
                dtp_ngayKT.Value = DateTime.Parse(ngayKT);
                cbo_stBH.Text = stBH;
            }
        }
        private bool validateDate = false;

        private void dtp_ngayBD_ValueChanged(object sender, EventArgs e)
        {
            if (!validateDate)
            {
                return;
            }

            DateTime ngayBatDau = dtp_ngayBD.Value;
            DateTime ngayKetThuc = dtp_ngayKT.Value;

            // Kiểm tra số ngày chênh lệch giữa ngày bắt đầu và ngày kết thúc
            TimeSpan KtraNgay = ngayKetThuc.Subtract(ngayBatDau);
            if (Math.Abs(KtraNgay.Days) < 5 * 365) // Kiểm tra nếu số ngày chênh lệch nhỏ hơn 5 năm (365 ngày x 5)
            {
                MessageBox.Show("Ngày bắt đầu và ngày kết thúc phải cách nhau ít nhất 5 năm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dtp_ngayKT_ValueChanged(object sender, EventArgs e)
        {
            if (!validateDate)
            {
                return;
            }

            DateTime ngayBatDau = dtp_ngayBD.Value;
            DateTime ngayKetThuc = dtp_ngayKT.Value;

            // Kiểm tra số ngày chênh lệch giữa ngày bắt đầu và ngày kết thúc
            TimeSpan KtraNgay = ngayKetThuc.Subtract(ngayBatDau);
            if (Math.Abs(KtraNgay.Days) < 5 * 365) // Kiểm tra nếu số ngày chênh lệch nhỏ hơn 5 năm (365 ngày x 5)
            {
                MessageBox.Show("Ngày bắt đầu và ngày kết thúc phải cách nhau ít nhất 5 năm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_timkiem_Click(object sender, EventArgs e)
        {
            string tenKH = txt_timkiem.Text.Trim();

            if (string.IsNullOrEmpty(tenKH))
            {
                return; // Không thực hiện tìm kiếm nếu txt_timkiem trống
            }

            dgv_HopDong.Rows.Clear();

            var filter = Builders<BsonDocument>.Filter.Regex("Khachhang.Tenkh", new BsonRegularExpression(tenKH, "i"));
            var documents = collection.Find(filter).ToList();

            foreach (var document in documents)
            {
                // tìm trong document
                var maHD = document.GetValue("_id").AsObjectId.ToString();
                var ngayKy = document.GetValue("Ngaybatdau").ToUniversalTime().ToUniversalTime().ToLocalTime().ToString("dd/MM/yyyy");
                var ngayKT = document.GetValue("Ngayketthuc").ToUniversalTime().ToUniversalTime().ToLocalTime().ToString("dd/MM/yyyy");
                var loaiBH = document["BaoHiem"]["Tenbaohiem"].AsString;
                var giaTri = document["Sotienbaohiem"].IsDouble ? document["Sotienbaohiem"].AsDouble.ToString() : document["Sotienbaohiem"].AsString;
                var tenKHResult = document["Khachhang"]["Tenkh"].AsString;
                var diaChi = document["Khachhang"]["Diachi"].AsString;
                var sdt = document["Khachhang"]["SDT"].AsString;
                var gioiTinh = document["Khachhang"]["Gioitinh"].AsString;
                var ngheNghiep = document["Khachhang"]["Nghenghiep"].AsString;

                // Hiện dữ liệu lên datagridview
                dgv_HopDong.Rows.Add(maHD, ngayKy, ngayKT, loaiBH, giaTri, tenKHResult, diaChi, sdt, gioiTinh, ngheNghiep);
            }

            ClearTextBoxes();
        }
    }
}
